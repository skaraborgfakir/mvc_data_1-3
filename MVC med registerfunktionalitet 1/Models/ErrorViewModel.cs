using System;

namespace Kartotek.Modeller
{
    public class ErrorViewModel
    {
	public string RequestId { get; set; }

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
