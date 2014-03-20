namespace Castle.MicroKernel.Lifestyle
{
	/// <summary>
	/// a custom Lifestyle, it will inherit from the standard class so if the 
	/// TrulyTransientReleasePolicy policy isn't used these objects are handled 
	/// as standard transient objects 
	/// </summary>
	public class TrulyTransientLifestyleManager : TransientLifestyleManager
	{

	}
}
