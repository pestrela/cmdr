﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace cmdr.Editor.ViewModels.Conditions  //to change
{
    public class CommandsReportEditorViewModel : ViewModelBase
    {  
        public ObservableCollection<ConditionTupleViewModel> Descriptions { get; private set; }


        public CommandsReportEditorViewModel(IEnumerable<MappingViewModel> mappings)
        {
            var conditionTuples = mappings
                .Where(m => !String.IsNullOrWhiteSpace(m.Conditions.ToString()))
                .GroupBy(c => c.Conditions.ToString())
                .Select(t => new ConditionTupleViewModel(t.ToList()))
                .ToList();
            Descriptions = new ObservableCollection<ConditionTupleViewModel>(conditionTuples);
        }
    }
}
