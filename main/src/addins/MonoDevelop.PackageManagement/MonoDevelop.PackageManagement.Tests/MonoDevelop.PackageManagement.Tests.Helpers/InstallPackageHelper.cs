﻿//
// InstallPackageHelper.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using MonoDevelop.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement.Tests.Helpers
{
	class InstallPackageHelper
	{
		InstallPackageAction action;

		public FakePackage TestPackage = new FakePackage () {
			Id = "Test"
		};

		public FakePackageRepository PackageRepository = new FakePackageRepository ();
		public List<PackageOperation> PackageOperations = new List<PackageOperation> ();

		public InstallPackageHelper (InstallPackageAction action)
		{
			this.action = action;
		}

		public void InstallTestPackage ()
		{
			action.Package = TestPackage;
			action.Operations = PackageOperations;
			action.IgnoreDependencies = IgnoreDependencies;
			action.AllowPrereleaseVersions = AllowPrereleaseVersions;
			action.Execute ();
		}

		public FakePackage AddPackageInstallOperation ()
		{
			var package = new FakePackage ("Package to install");
			var operation = new PackageOperation (package, PackageAction.Install);
			PackageOperations.Add (operation);
			return package;
		}

		public PackageSource PackageSource = new PackageSource ("http://monodevelop/packages");
		public bool IgnoreDependencies;
		public bool AllowPrereleaseVersions;
		public SemanticVersion Version;

		public void InstallPackageById (string packageId)
		{
			action.PackageId = packageId;
			action.PackageVersion = Version;
			action.IgnoreDependencies = IgnoreDependencies;
			action.AllowPrereleaseVersions = AllowPrereleaseVersions;

			action.Execute ();
		}
	}
}

