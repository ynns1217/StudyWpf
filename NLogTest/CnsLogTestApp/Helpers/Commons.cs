using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnsLogTestApp
{
	class Commons
	{
		// NLog용 정적(static) 인스턴스 생성

		// stattic은 보통 대문자로 사용한다 = 산업계표준
		public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();
	}
}
