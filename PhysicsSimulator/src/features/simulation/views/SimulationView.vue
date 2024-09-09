<script setup lang="ts">
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Button } from '@/components/ui/button'
import {ref} from "vue";
import CircleLoader from '@/components/icons/CircleLoader.vue'
import { State } from '@/shared/utils/State'
import type { Result } from '@/shared/utils/Result'
import { SimulationErrors, simulationService } from '@/features/simulation/services/simulationService'
import { toast } from '@/components/ui/toast'
import { LineChart } from '@/components/ui/chart-line'
import { Separator } from '@/components/ui/separator'
import {FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage} from '@/components/ui/form'
import * as z from 'zod'
import { toTypedSchema } from '@vee-validate/zod'
import { type FormValidationResult, useForm } from 'vee-validate'
import type {
  DataPoint,
  RunSimulationResponse
} from '@/features/simulation/services/commands/RunSimulation/RunSimulationResponse'
import type { RunSimulationCommand } from '@/features/simulation/services/commands/RunSimulation/RunSimulationCommand'

const state = ref<State>(State.Idle);
const formSchema = toTypedSchema(z.object({
  AirTemperature: z.number().min(273).max(400),
  AirVelocity: z.number().min(1).max(20),
  SolarPanelLength: z.number().min(1).max(20),
  SolarPanelSurfaceTemperature: z.number().min(250).max(375),
  StorageTankHeight: z.number().min(1).max(20),
  SolarIrradiation: z.number().min(650).max(850),
  StorageTankRadius: z.number().min(1).max(20),
  InitialTankFluidTemperature: z.number().min(285).max(370),
  TankFluidMassFlowRate: z.number().min(0.00001).max(2),
}))

const { values, validate } = useForm({
  validationSchema: formSchema,
  keepValuesOnUnmount: false,
})

const validateFields = async (): FormValidationResult => {
  return validate();
}

const errorMessage = ref<string>("");
const runSimulationForm = ref<HTMLFormElement | null>(null)
const submitForm = () => {

  if (runSimulationForm.value == null) {
    return;
  }

  // we dispatch a submit event because our submit button is
  // outside the form element
  runSimulationForm.value.dispatchEvent(new Event('submit', { cancelable: true }));
}

const runSimulation = async () => {
  try {
    state.value = State.Idle;

    state.value = State.Submitting;

    const validationResult: FormValidationResult<any, any> = await validateFields();
    if (!validationResult.valid) {
      state.value = State.Error
      return;
    }

    const command: RunSimulationCommand = generateCommand();
    const result: Result<RunSimulationResponse, Error> = await simulationService.runSimulationAsync(command)

    if (!result.isSuccess) {
      handleError(result.error)
      return
    }

    data.value = result.value.DataPoints;

    state.value = State.Success;

  } catch (error) {
    handleUnexpectedError(error)
  }
}

const handleError = (errorResponse: any) => {

  if (errorResponse.name == SimulationErrors.conflict) {
    errorMessage.value = errorResponse.message;
  }
  else if (errorResponse.name == SimulationErrors.unprocessableEntity) {
    errorMessage.value = errorResponse.message;
  }
  else {
    toast({
      title: 'Uh oh! Something went wrong.',
      description: errorResponse.message,
      variant: 'destructive',
    });
  }

  state.value = State.Error;
}

const handleUnexpectedError = (error: any) => {
  toast({
    title: 'Uh oh! Something went wrong.',
    description: error.description,
    variant: 'destructive',
  });

  state.value = State.Error;
}

const generateCommand = () : RunSimulationCommand => {
  const command: RunSimulationCommand = {
    airTemperature: values.AirTemperature ?? 0,
    airVelocity: values.AirVelocity ?? 0,
    solarPanelLength: values.SolarPanelLength ?? 0,
    solarPanelSurfaceTemperature: values.SolarPanelSurfaceTemperature ?? 0,
    solarIrradiation: values.SolarIrradiation ?? 0,
    storageTankHeight: values.StorageTankHeight ?? 0,
    storageTankRadius: values.StorageTankRadius ?? 0,
    initialTankFluidTemperature: values.InitialTankFluidTemperature ?? 0,
    tankFluidMassFlowRate: values.TankFluidMassFlowRate ?? 0
  }
  return command;
}

const data = ref<DataPoint[] | undefined>();
</script>

<template>
  <div class="container">

    <div class="gradient-background"></div>

    <div class="content-container">

      <Card class="card">
        <CardHeader>
          <CardTitle>Simulation Parameters</CardTitle>
        </CardHeader>
        <CardContent>
          <form ref="runSimulationForm" @submit.prevent="runSimulation">
            <div class="space-y-1">
              <h4 class="text-sm font-medium leading-none">Environment</h4>
              <p class="text-sm text-muted-foreground">Sun and ambient air.</p>
            </div>
            <div class="grid grid-cols-3 gap-4 mb-2 mt-3">

              <div class="gap-2">
                <FormField name="AirTemperature" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Air Temperature (K)</FormLabel>
                    <FormControl>
                        <Input id="airTemperature" type="number" placeholder="0.0" v-bind="componentField" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

              <div class="gap-2">
                <FormField name="AirVelocity" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Air Velocity (m/s)</FormLabel>
                    <FormControl>
                      <Input id="airVelocity" type="number" placeholder="0.0" v-bind="componentField" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

              <div class="gap-2">
                <FormField name="SolarIrradiation" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Solar Irradiation (W/m^s)</FormLabel>
                    <FormControl>
                      <Input id="solarIrradiation" type="number" placeholder="0.0" v-bind="componentField" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

            </div>

            <Separator class="my-4" />

            <div class="space-y-1">
              <h4 class="text-sm font-medium leading-none">Solar Panel</h4>
            </div>
            <div class="grid grid-cols-2 gap-4 mb-2 mt-3">

              <div class="gap-2">
                <FormField name="SolarPanelSurfaceTemperature" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Surface Temp (K)</FormLabel>
                    <FormControl>
                      <Input id="solarPanelSurfaceTemperature" type="number" placeholder="0.0" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

              <div class="gap-2">
                <FormField name="SolarPanelLength" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Length (m)</FormLabel>
                    <FormControl>
                      <Input id="solarPanelLength" type="number" placeholder="0.0" v-bind="componentField" />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

            </div>
            <Separator class="my-4" />

            <div class="space-y-1">
              <h4 class="text-sm font-medium leading-none">Storage Tank</h4>
              <p class="text-sm text-muted-foreground">Liquid in tank is water.</p>
            </div>
            <div class="grid grid-cols-2 gap-4 mb-2 mt-3">

              <div class="gap-2">
                <FormField name="StorageTankHeight" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Tank Height (m)</FormLabel>
                    <FormControl>
                      <Input id="storageTankHeight" type="number" placeholder="0.0" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

              <div class="gap-2">
                <FormField name="StorageTankRadius" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Tank Radius (m)</FormLabel>
                    <FormControl>
                      <Input id="storageTankRadius" type="number" placeholder="0.0" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

            </div>

            <div class="grid grid-cols-2 gap-4 mb-2 mt-3">

              <div class="gap-2">
                <FormField name="InitialTankFluidTemperature" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Initial Fluid Temp (K)</FormLabel>
                    <FormControl>
                      <Input id="initialTankFluidTemperature" type="number" placeholder="0.0" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

              <div class="gap-2">
                <FormField name="TankFluidMassFlowRate" v-slot="{ componentField }">
                  <FormItem class="flex flex-col">
                    <FormLabel>Fluid Mass Flow Rate (kg/s)</FormLabel>
                    <FormControl>
                      <Input id="tankFluidMassFlowRate" type="number" placeholder="0.0" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                </FormField>
              </div>

            </div>
          </form>

        </CardContent>
        <CardFooter class="flex justify-end">
          <Button @click="submitForm" :disabled="state == State.Submitting">
            <CircleLoader v-if="state == State.Submitting" class="submit-button__circle-loader"></CircleLoader>
            <span v-else>Simulate</span>
          </Button>
        </CardFooter>
      </Card>

      <Card v-if="data" class="chart-card">
        <LineChart
          class="line-chart"
          :data="data"
          index="Time"
          :categories="['TankTemperature']"
          :y-formatter="(tick, i) => {
          return typeof tick === 'number'
            ? `${new Intl.NumberFormat('us').format(tick).toString()} (K)`
            : ''
        }"
        />
      </Card>

    </div>
  </div>

</template>

<style scoped>
.container {
  width: 100vw;
  height: 100vh;
}

.content-container {
  display: flex;
  flex-direction: column;
  justify-content: start;
  align-items: center;
  height: 100vh;
}

.content-container > *:first-child {
  margin-top: 2rem;
}

.card {
  max-height: 750px;
  min-height: 650px;
  min-width: 600px;
  max-width: 650px;
}

.chart-card {
  width: 45%;
  min-width: 45%;
}

.line-chart {
  margin-bottom: 2rem;
  margin-top: 2rem;
}

.gradient-background {
  z-index: -10;
  width: 100vw;
  height: 100vh;
  position: absolute;
  overflow: hidden;
  background: linear-gradient(40deg, #000, rgba(var(--color1), 0.3));

  top: 0;
  left: 0;
}

@media screen and (min-width: 800px) {
  .content-container {
    justify-content: space-between;
    flex-direction: row;
  }

  .content-container > *:first-child {
    margin-top: 0;
  }
}

/* background: linear-gradient(40deg, var(--color-bg1), var(--color-bg2)); */
</style>