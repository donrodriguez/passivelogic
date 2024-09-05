using PassiveLogicPhysicsSimulation.Empirical;
using PassiveLogicPhysicsSimulation.SystemComponents.Air;


ThermophysicalProperties properties = AirProperties.Instance.AirPropertiesAtTempInKelvin(200M);
Console.WriteLine(properties.Prandtl);

// 