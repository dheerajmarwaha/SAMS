namespace IMSCore.Infra.Framework.Contracts
{
    public enum InstanceLifetimeOptions
    {
        Transient = 1,
        PerThreadLife,
        PerContainerLife
    }
}
