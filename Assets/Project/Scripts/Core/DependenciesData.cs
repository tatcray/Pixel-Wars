using System;
using Dependencies;


[Serializable]
public class DependenciesData
{
    public UpgradesData upgradesData;
    public WallConfig wallConfig;
    public WeaponReferences weaponReferences;
    public CrosshairReferences crosshairReferences;
    public CameraDependencies cameraDependencies;
    public ConverterDependencies converterDependencies;
    public UIDependencies uiDependencies;
    public EnvironmentDependencies environmentDependencies;
    public ServicesDependencies servicesDependencies;
}