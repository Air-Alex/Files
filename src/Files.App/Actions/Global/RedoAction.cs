﻿// Copyright (c) Files Community
// Licensed under the MIT License.

namespace Files.App.Actions
{
	internal sealed partial class RedoAction : ObservableObject, IAction
	{
		private readonly IContentPageContext context;

		public string Label
			=> Strings.Redo.GetLocalizedResource();

		public string Description
			=> Strings.RedoDescription.GetLocalizedResource();

		public HotKey HotKey
			=> new(Keys.Y, KeyModifiers.Ctrl);

		public bool IsExecutable =>
			context.ShellPage is not null &&
			context.PageType is not ContentPageTypes.SearchResults;

		public RedoAction()
		{
			context = Ioc.Default.GetRequiredService<IContentPageContext>();

			context.PropertyChanged += Context_PropertyChanged;
		}

		public Task ExecuteAsync(object? parameter = null)
		{
			return context.ShellPage!.StorageHistoryHelpers.TryRedo();
		}

		private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IContentPageContext.ShellPage):
				case nameof(IContentPageContext.PageType):
					OnPropertyChanged(nameof(IsExecutable));
					break;
			}
		}
	}
}
