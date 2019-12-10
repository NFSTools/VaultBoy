using System;
using System.Diagnostics;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using VaultBoy.Editing;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultBoy.ViewModel.Tabs
{
    public class CollectionEditorViewModel : TabModelBase, IEquatable<CollectionEditorViewModel>
    {
        private VLTDataProxy _proxy;
        private VLTCollection _collection;

        public CollectionEditorViewModel(VLTCollection collection)
        {
            _collection = collection;
            EditFieldsCommand = new RelayCommand(ExecuteEditFields);
            ChangeVaultCommand = new RelayCommand(ExecuteChangeVault);
            UpdateDataProxy();
        }

        public RelayCommand EditFieldsCommand { get; }
        public RelayCommand ChangeVaultCommand { get; }

        public VLTCollection Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                RaisePropertyChanged();
                UpdateDataProxy();
            }
        }

        public VLTDataProxy Proxy
        {
            get => _proxy;
            set
            {
                _proxy = value;
                RaisePropertyChanged();
            }
        }

        public override string TabTitle => $"Editing - {Collection.ShortPath}";

        public override EFontAwesomeIcon TabIcon => EFontAwesomeIcon.Regular_FolderOpen;

        private void ExecuteEditFields()
        {
            //CollectionFieldEditor collectionFieldEditor = new CollectionFieldEditor(Collection);

            //if (collectionFieldEditor.ShowDialog() != true) return;

            //foreach (var fieldEntry in collectionFieldEditor.Entries)
            //{
            //    if (!fieldEntry.IsEnabled)
            //    {
            //        if (fieldEntry.IsBase)
            //            throw new Exception("Elite hacking detected");
            //        if (Collection.DataRow.ContainsKey(fieldEntry.Field.Key))
            //            Collection.DataRow.Remove(fieldEntry.Field.Key);
            //    }
            //    else
            //    {
            //        if (!Collection.DataRow.ContainsKey(fieldEntry.Field.Key))
            //        {
            //            Collection.DataRow[fieldEntry.Field.Key] =
            //                TypeRegistry.AutoCreate(Collection.Vault.Database.Game, fieldEntry.Field, Collection.Class,
            //                    Collection);

            //            if (fieldEntry.IsArray && Collection.DataRow[fieldEntry.Field.Key] is VLTArrayType array)
            //            {
            //                array.Items = new VLTBaseType[fieldEntry.ArrayLength];
            //            }
            //        }
            //        else if (fieldEntry.IsArray && Collection.DataRow[fieldEntry.Field.Key] is VLTArrayType array)
            //        {
            //            VLTBaseType[] items = array.Items;
            //            if (fieldEntry.ArrayLength != items.Length)
            //            {
            //                Array.Resize(ref items, fieldEntry.ArrayLength);
            //            }

            //            array.Items = items;
            //            Collection.DataRow[fieldEntry.Field.Key] = array;
            //        }
            //    }
            //}

            //UpdateDataProxy();
        }

        private void ExecuteChangeVault()
        {
            //VaultSelector vs = new VaultSelector();

            //if (vs.ShowDialog() == true)
            //{
            //    Debugger.Break();
            //}
        }

        private void UpdateDataProxy()
        {
            VLTDataProxy proxy = SimpleIoc.Default.GetInstanceWithoutCaching<VLTDataProxy>();
            proxy.SetCollection(Collection);
            Proxy = proxy;
        }

        #region IEquatable Members

        public bool Equals(CollectionEditorViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Collection, other.Collection);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CollectionEditorViewModel)obj);
        }

        public override int GetHashCode()
        {
            return (Collection != null ? Collection.GetHashCode() : 0);
        }

        public static bool operator ==(CollectionEditorViewModel left, CollectionEditorViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CollectionEditorViewModel left, CollectionEditorViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
