namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composes a runtime from a manifest.</summary>
public interface IFsmCos
{
    RuntimeAssembly Execute(RuntimeManifest manifest);
}
