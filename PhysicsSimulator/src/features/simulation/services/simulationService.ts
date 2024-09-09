import axios from 'axios';
import { StatusCodes } from "http-status-codes";
import {failure, success} from '@/shared/utils/Result'
import {endpoint} from "@/shared/services/ServiceSettings";
import type {Result} from "@/shared/utils/Result";
import type { RunSimulationCommand } from '@/features/simulation/services/commands/RunSimulation/RunSimulationCommand'
import { ApiError } from '@/shared/utils/ApiError'
import type { RunSimulationResponse } from '@/features/simulation/services/commands/RunSimulation/RunSimulationResponse'

class SimulationService {

  private static pathBase = "/api/simulations";

  private static Paths = {
    runSimulation: () : string => `${SimulationService.pathBase}/run`,
  }

  public async runSimulationAsync(command: RunSimulationCommand): Promise<Result<RunSimulationResponse, Error>> {

    try {
      const url = endpoint(SimulationService.Paths.runSimulation());
      const response = await axios.post(url, command);
      if (response.status == StatusCodes.CREATED) {
        return success(response.data);
      }
      return success(response.data);

    } catch (error: any) {
      if (error.response.status == StatusCodes.NOT_FOUND) {
        return failure(new ApiError(StatusCodes.NOT_FOUND, SimulationErrors.notFound, error.response.data.detail));
      }
      else if (error.response.status == StatusCodes.UNPROCESSABLE_ENTITY) {
        return failure(new ApiError(StatusCodes.UNPROCESSABLE_ENTITY, SimulationErrors.unprocessableEntity, error.response.data.detail));
      }

      return failure(new Error("Simulation failed, please try again."));
    }
  }

}

export enum SimulationErrors {
  forbidden = "Forbidden",
  unauthorized = "Unauthorized",
  conflict = "Conflict",
  unprocessableEntity = "UnprocessableEntity",
  serverError = "ServerError",
  notFound = "NotFound"
}

export const simulationService = new SimulationService();