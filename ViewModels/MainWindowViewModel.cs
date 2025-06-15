using InterpolApp.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text.Json;

namespace InterpolApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _lastWantedPersonId;
        private int _lastWarrantId;
        private int _lastGroupId;

        private ObservableCollection<Suspect> _suspects;
        private ObservableCollection<Suspect> _archivedSuspects;
        private Suspect _selectedSuspect;
        private Suspect _selectedArchivedSuspect;
        
        private ObservableCollection<Warrant> _warrants;
        private Warrant _selectedWarrant;
        private Suspect _selectedSuspectForWarrant;

        private ObservableCollection<CriminalGroup> _criminalGroups;
        private CriminalGroup _selectedCriminalGroup;
        
        private ObservableCollection<Suspect> _filteredSuspects;
        private bool _isEditMode;

        public ObservableCollection<Suspect> Suspects
        {
            get => _suspects;
            set => this.RaiseAndSetIfChanged(ref _suspects, value);
        }

        public ObservableCollection<Suspect> ArchivedSuspects
        {
            get => _archivedSuspects;
            set => this.RaiseAndSetIfChanged(ref _archivedSuspects, value);
        }
        
        public ObservableCollection<Warrant> Warrants
        {
            get => _warrants;
            set => this.RaiseAndSetIfChanged(ref _warrants, value);
        }
        
        public ObservableCollection<CriminalGroup> CriminalGroups
        {
            get => _criminalGroups;
            set => this.RaiseAndSetIfChanged(ref _criminalGroups, value);
        }

        public Suspect SelectedSuspect
        {
            get => _selectedSuspect;
            set => this.RaiseAndSetIfChanged(ref _selectedSuspect, value);
        }

        public Suspect SelectedArchivedSuspect
        {
            get => _selectedArchivedSuspect;
            set => this.RaiseAndSetIfChanged(ref _selectedArchivedSuspect, value);
        }
        
        public Warrant SelectedWarrant
        {
            get => _selectedWarrant;
            set => this.RaiseAndSetIfChanged(ref _selectedWarrant, value);
        }
        
        public Suspect SelectedSuspectForWarrant
        {
            get => _selectedSuspectForWarrant;
            set => this.RaiseAndSetIfChanged(ref _selectedSuspectForWarrant, value);
        }

        public CriminalGroup SelectedCriminalGroup
        {
            get => _selectedCriminalGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedCriminalGroup, value);
        }
        
        public bool IsEditMode
        {
            get => _isEditMode;
            set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
        }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }
        public ReactiveCommand<Unit, Unit> ArchiveCommand { get; }
        public ReactiveCommand<Unit, Unit> AddWarrantCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveWarrantCommand { get; }
        public ReactiveCommand<Unit, Unit> EditWarrantCommand { get; }
        public ReactiveCommand<Unit, Unit> AddCriminalGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveCriminalGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCriminalGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> UnarchiveCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveArchivedCommand { get; }

        public MainWindowViewModel()
        {
            Suspects = new ObservableCollection<Suspect>();
            ArchivedSuspects = new ObservableCollection<Suspect>();
            Warrants = new ObservableCollection<Warrant>();
            CriminalGroups = new ObservableCollection<CriminalGroup>();

            AddCommand = ReactiveCommand.Create(AddSuspect);
            RemoveCommand = ReactiveCommand.Create(RemoveSuspect);
            EditCommand = ReactiveCommand.Create(EditSuspect);
            ArchiveCommand = ReactiveCommand.Create(ArchiveSuspect);

            AddWarrantCommand = ReactiveCommand.Create(AddWarrant);
            RemoveWarrantCommand = ReactiveCommand.Create(RemoveWarrant);
            EditWarrantCommand = ReactiveCommand.Create(EditWarrant);

            AddCriminalGroupCommand = ReactiveCommand.Create(AddCriminalGroup);
            RemoveCriminalGroupCommand = ReactiveCommand.Create(RemoveCriminalGroup);
            EditCriminalGroupCommand = ReactiveCommand.Create(EditCriminalGroup);
            
            UnarchiveCommand = ReactiveCommand.Create(UnarchiveSuspect);
            RemoveArchivedCommand = ReactiveCommand.Create(RemoveArchivedSuspect);

            LoadData();
        }
        
        public ObservableCollection<Suspect> FilteredSuspects
        {
            get => _filteredSuspects;
            set => this.RaiseAndSetIfChanged(ref _filteredSuspects, value);
        }
        
        
        private void AddSuspect()
        {
            var newSuspect = new Suspect { WantedPersonId = GetNextWantedPersonId() };
            Suspects.Add(newSuspect);
            SelectedSuspect = newSuspect;
          
            SaveData();
        }

        private void RemoveSuspect()
        {
            if (SelectedSuspect != null)
            {
                Suspects.Remove(SelectedSuspect);
                SaveData(); 
            }
        }

        private void ArchiveSuspect()
        {
            if (SelectedSuspect != null)
            {
                var suspectToArchive = Suspects.FirstOrDefault(s => s.WantedPersonId == SelectedSuspect.WantedPersonId);

                if (suspectToArchive != null)
                {
                    Suspects.Remove(suspectToArchive);
                    ArchivedSuspects.Add(suspectToArchive);
                    SaveData();
                }
            }
        }

        private void EditSuspect()
        {
            IsEditMode = !IsEditMode;
            SaveData();
        }

        private int GetNextWantedPersonId()
        {
            return ++_lastWantedPersonId;
        }

        private void AddWarrant()
        {
            var newWarrant = new Warrant
            {
                WarrantId = GetNextWarrantId(),
                Description = "New Warrant Description",
                SuspectName = ""
            };
            Warrants.Add(newWarrant);
            SelectedWarrant = newWarrant;
            SaveData();
        }

        private void RemoveWarrant()
        {
            if (SelectedWarrant != null)
            {
                Warrants.Remove(SelectedWarrant);
                SaveData();
            }
        }

        private void EditWarrant()
        {
            IsEditMode = !IsEditMode;
            SaveData();
        }

        private int GetNextWarrantId()
        {
            return ++_lastWarrantId;
        }

        private void AddCriminalGroup()
        {
            var newGroup = new CriminalGroup
            {
                GroupId = GetNextGroupId(),
                Name = "New Group Name",
                Description = "New Group Description"
            };
            CriminalGroups.Add(newGroup);
            SelectedCriminalGroup = newGroup;
            SaveData();
        }

        private void RemoveCriminalGroup()
        {
            if (SelectedCriminalGroup != null)
            {
                CriminalGroups.Remove(SelectedCriminalGroup);
                SaveData();
            }
        }

        private void EditCriminalGroup()
        {
            IsEditMode = !IsEditMode;
            SaveData();
        }

        private int GetNextGroupId()
        {
            return ++_lastGroupId;
        }
        

        private void SaveData()
        {
            var dataModel = new DataModel
            {
                Suspects = Suspects.ToList(),
                ArchivedSuspects = ArchivedSuspects.ToList(),
                Warrants = Warrants.ToList(),
                CriminalGroups = CriminalGroups.ToList(),
                LastWantedPersonId = _lastWantedPersonId,
                LastWarrantId = _lastWarrantId,
                LastGroupId = _lastGroupId
            };

            var json = JsonSerializer.Serialize(dataModel);
            File.WriteAllText("data.json", json);
        }

        private void LoadData()
        {
            if (File.Exists("data.json"))
            {
                var json = File.ReadAllText("data.json");
                var dataModel = JsonSerializer.Deserialize<DataModel>(json);

                if (dataModel != null)
                {
                    Suspects = new ObservableCollection<Suspect>(dataModel.Suspects ?? new List<Suspect>());
                    ArchivedSuspects = new ObservableCollection<Suspect>(dataModel.ArchivedSuspects ?? new List<Suspect>());
                    Warrants = new ObservableCollection<Warrant>(dataModel.Warrants ?? new List<Warrant>());
                    CriminalGroups = new ObservableCollection<CriminalGroup>(dataModel.CriminalGroups ?? new List<CriminalGroup>());
                    _lastWantedPersonId = dataModel.LastWantedPersonId;
                    _lastWarrantId = dataModel.LastWarrantId;
                    _lastGroupId = dataModel.LastGroupId;
                }
            }
            else
            {
                InitializeEmptyData();
            }
        }

        private void InitializeEmptyData()
        {
            _lastWantedPersonId = 0;
            _lastWarrantId = 0;
            _lastGroupId = 0;
            Suspects = new ObservableCollection<Suspect>();
            ArchivedSuspects = new ObservableCollection<Suspect>();
            Warrants = new ObservableCollection<Warrant>();
            CriminalGroups = new ObservableCollection<CriminalGroup>();
        }
        
        private void UnarchiveSuspect()
        {
            if (SelectedArchivedSuspect != null)
            {
                Suspects.Add(SelectedArchivedSuspect);
                ArchivedSuspects.Remove(SelectedArchivedSuspect);   
                SelectedSuspect = SelectedArchivedSuspect;
                SaveData();
            }
        }

        private void RemoveArchivedSuspect()
        {
            if (SelectedArchivedSuspect != null)
            {
                ArchivedSuspects.Remove(SelectedArchivedSuspect);
                SaveData();
            }
        }
    }
}
