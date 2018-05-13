using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class LinqExtension {

	public static  IEnumerable<T> DistinctBy<T>(this IEnumerable<T> list, Func<T, object> propertySelector)
	{
	   return list.GroupBy(propertySelector).Select(x => x.First());
	}

}
