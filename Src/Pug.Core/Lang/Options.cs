using System;
using System.Collections.Generic;
using System.Linq;

namespace Pug.Lang
{
	public record Options<TFirst, TSecond>
	{
		public Options()
		{
		}

		public Options( Option<TFirst> first, Option<TSecond> second )
		{
			First = first;
			Second = second;
		}

		public Option<TFirst> First { get; set; }

		public Option<TSecond> Second { get; set; }

		protected virtual IEnumerable<IOption> All => new IOption[] { First, Second };

		public bool Only<TOption>()
		{
			if( Second is null && First is TOption )
				return true;

			return First is null && Second is TOption;
		}

		public bool Contain<TOption>()
		{
			if( First is TOption )
				return true;

			return Second is TOption;
		}

		public void For( Action<TFirst> first, Action<TSecond> second )
		{
			First?.AsParameterOf( first );

			Second?.AsParameterOf( second );
		}

		public (Result<TFirstResult>, Result<TSecondResult>) Map<TFirstResult, TSecondResult>(
			Func<TFirst, TFirstResult> first, Func<TSecond, TSecondResult> second )
		{
			return ( First?.Map( first ), Second?.Map( second ) );
		}

		public TOption Get<TOption>()
		{
			IOption option = All.FirstOrDefault( x => x.Is<TOption>() );

			return option is null ? default(TOption) : ( option as Option<TOption> ).Value;
		}
	}
}