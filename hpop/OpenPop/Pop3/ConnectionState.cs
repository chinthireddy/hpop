﻿namespace OpenPop.Pop3
{
	/// <summary>
	/// Some of these states are defined by <a href="http://www.rfc-editor.org/rfc/rfc1939.txt">RFC 1939</a>.
	/// Which commands that are allowed in which state can be seen in the same RFC.
	/// 
	/// Used to keep track of which state the <see cref="Pop3Client"/> is in.
	/// </summary>
	internal enum ConnectionState
	{
		/// <summary>
		/// This is when the Pop3Client is not even connected to the server
		/// </summary>
		Disconnected,

		/// <summary>
		/// This is when the server is awaiting user credentials
		/// </summary>
		Authorization,

		/// <summary>
		/// This is when the server has been given the user credentials, and we are allowed
		/// to use commands specific to this users mail drop
		/// </summary>
		Transaction
	}
}