import axios from 'axios';
import { StatusCodes } from "http-status-codes";
import {failure, success} from "@/utils/Result";
import {endpoint} from "@/shared/services/ServiceSettings";
import type {Result} from "@/shared/utils/Result";
import type { RunSimulationCommand } from '@/features/simulation/services/commands/RunSimulation/RunSimulationCommand'

class SimulationService {

  private static pathBase = "/v1/simulations";

  private static Paths = {
    // runSimulation: (questId: string): string => `${QuestService.pathBase}/${questId}`,
  }

  public async runSimulationAsync(command: RunSimulationCommand, jwtToken: string): Promise<Result<CreateQuestResponse, Error>> {

    try {
      const url = endpoint(QuestService.pathBase);
      const config = {
        headers: {
          Authorization: `Bearer ${jwtToken}`
        }
      }

      const response = await axios.post(url, command, config);
      if (response.status == StatusCodes.CREATED) {
        return success(response.data);
      }
      return success(response.data);

    } catch (error: any) {
      if (error.response.status == StatusCodes.NOT_FOUND) {
        return failure(new ApiError(StatusCodes.NOT_FOUND, QuestErrors.notFound, error.response.data.detail));
      }
      else if (error.response.status == StatusCodes.UNPROCESSABLE_ENTITY) {
        return failure(new ApiError(StatusCodes.UNPROCESSABLE_ENTITY, QuestErrors.unprocessableEntity, error.response.data.detail));
      }
      else if (error.response.status == StatusCodes.UNAUTHORIZED) {
        return failure(new ApiError(StatusCodes.UNAUTHORIZED, QuestErrors.unauthorized, error.response.data.detail));
      }

      return failure(new Error("Quest creation failed, please try again."));
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