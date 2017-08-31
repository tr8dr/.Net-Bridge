// 
// General:
//      This file is part of .NET Bridge
//
// Copyright:
//      2010 Jonathan Shore
//      2017 Jonathan Shore and Contributors
//
// License:
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at:
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
//

using System;

namespace bridge.common.serialization
{
	/// <summary>
	/// Interface for classes that implement persistance / serialization into some hierarchical tagged format
	/// </summary>
	public interface IPersist<Format>
	{
		
		/// <summary>
		/// Gets and sets the state into the object into the appropriate format
		/// </summary>
		Format				State			{ get; set; }
	}

	/// <summary>
	/// Interface for classes that implement persistance / serialization into some hierarchical tagged format
	/// </summary>
	public interface IPersistRO<Format>
	{

		/// <summary>
		/// Gets the state of the object in required format
		/// </summary>
		Format				State			{ get; }
	}
			
}

