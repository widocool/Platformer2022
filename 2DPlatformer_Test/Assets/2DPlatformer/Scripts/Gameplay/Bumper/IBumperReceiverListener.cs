namespace GSGD2.Gameplay
{
	/// <summary>
	/// Interface used to transmit when a bump occur to a component. See <see cref="Bumper"/> and <see cref="BumperReceiver"/>. Since the bump interact with rigidbody, it can be used to the others movement component that a bump occur.
	/// </summary>
	public interface IBumperReceiverListener
	{
		void OnBump();
	}
}