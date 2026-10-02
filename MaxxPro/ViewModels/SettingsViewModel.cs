// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation.Collections;

namespace MaxxPro.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        private readonly IPropertySet settingValues;

        private const int DEFAULT_PORT = 11434;
        private const string DEFAIULT_MODEL = "gemma4:e2b";

        internal SettingsViewModel()
        {
            settingValues = ApplicationData.GetDefault().LocalSettings.Values;
        }

        internal double OllamaPort
        {
            get
            {
                if (settingValues["OllamaPort"] is int port)
                    return port;

                settingValues["OllamaPort"] = DEFAULT_PORT;
                return DEFAULT_PORT;
            }

            set
            {
                settingValues["OllamaPort"] = value;
                OnPropertyChanged();
            }
        }

        internal string ModelName
        {
            get
            {
                if (settingValues["Model"] is string { Length: >= 1 } model)
                {
                    if (!string.IsNullOrWhiteSpace(model))
                        return model;
                }

                settingValues["Model"] = DEFAIULT_MODEL;
                return DEFAIULT_MODEL;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    settingValues["Model"] = value;
                OnPropertyChanged();
            }
        }
    }
}
