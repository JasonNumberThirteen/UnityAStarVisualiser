using System.Text.RegularExpressions;
using System.Collections.Generic;

public class HeuristicTypeEnumDropdownUI : EnumDropdownUI<HeuristicManager, HeuristicType>
{
	protected override HeuristicType GetInitialValue() => HeuristicType.ManhattanDistance;
	protected override string GetLocalizationTableReferenceKey() => "Heuristic Settings Panel";

	protected override List<string> GetOptions()
	{
		var heuristicTypeNames = base.GetOptions();
		var adjustedHeuristicTypeNames = new List<string>();

		heuristicTypeNames.ForEach(heuristicTypeName => adjustedHeuristicTypeNames.Add(Regex.Replace(heuristicTypeName, "(\\B[A-Z])", " $1")));

		return adjustedHeuristicTypeNames;
	}

	protected override void OnEnumValueWasChanged(HeuristicType heuristicType)
	{
		if(component != null)
		{
			component.SetHeuristicType(heuristicType);
		}
	}
}