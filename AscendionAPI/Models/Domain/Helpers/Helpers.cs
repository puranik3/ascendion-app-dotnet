using System;
using System.Net.NetworkInformation;

namespace AscendionAPI.Models.Domain;

public class Helpers
{
	// In C#, you can provide a constructor constraint in a generic method or generic class using the new() constraint, but it only works for parameterless constructors. Unfortunately, C# does not directly support specifying a constructor with specific parameters as a constraint in generic types.
	// So we workaround using a Factory Delegate or Lambda Expression
	public static List<TDto> ToDtoList<TDomain, TDto>(IEnumerable<TDomain> listDomain, Func<TDomain, TDto> factory)
	{
		{
			List<TDto> listDto = new List<TDto>();

			foreach (var obj in listDomain)
			{
				listDto.Add(factory(obj));
			}

			return listDto;

		}
	}
}