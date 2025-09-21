public class SimulationTypeEnumDropdownUI : EnumDropdownUI<SimulationManager, SimulationType>
{
	protected override SimulationType GetInitialValue() => SimulationType.Timed;
	protected override string GetLocalizationTableReferenceKey() => "Simulation Settings Panel";

	protected override void OnEnumValueWasChanged(SimulationType simulationType)
	{
		if(component != null)
		{
			component.SetSimulationType(simulationType);
		}
	}
}