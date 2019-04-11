using Unity.Entities;

public struct MoveForward : ISharedComponentData { }

public class MoveForwardComponent : SharedComponentDataProxy<MoveForward> { }