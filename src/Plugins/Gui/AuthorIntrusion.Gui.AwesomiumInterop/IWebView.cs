namespace AuthorIntrusion.Gui.AwesomiumInterop
{
	public interface IWebView
	{
		/// <summary>
		/// Renders the HTML for the given view which is then handed to the WebControl
		/// for displaying to the user.
		/// </summary>
		/// <returns></returns>
		string RenderHtml();
	}
}