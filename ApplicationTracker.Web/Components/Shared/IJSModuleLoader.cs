namespace ApplicationTracker.Web.Components.Shared
{
	public interface IJSModuleLoader
	{
		/// <summary>
		/// Register modules in OnAfterRender to ensure that JSRuntime is available.
		/// </summary>
		/// <param name="modules"></param>
		/// <returns></returns>
		Task RegisterAsync(params ModuleType[] modules);
		ValueTask<T> InvokeAsync<T>(ModuleType module, string functionName, params object[] args);
		Task ReleaseAsync();

	}
}
