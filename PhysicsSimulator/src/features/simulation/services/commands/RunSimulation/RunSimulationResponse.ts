export interface RunSimulationResponse {
  DataPoints: DataPoint[];
}

export interface DataPoint {
  Time: number;
  TankTemperature: number;
}