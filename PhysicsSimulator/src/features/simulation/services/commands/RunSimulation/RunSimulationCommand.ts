export interface RunSimulationCommand {
  airTemperature: number;
  airVelocity: number;
  solarPanelLength: number;
  solarPanelSurfaceTemperature: number;
  solarIrradiation: number;
  storageTankHeight: number;
  storageTankRadius: number;
  initialTankFluidTemperature: number;
  tankFluidMassFlowRate: number;
}