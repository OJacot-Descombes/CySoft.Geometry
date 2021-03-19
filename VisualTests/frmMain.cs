using System;
using System.Linq;
using System.Windows.Forms;

namespace VisualTests
{
    public partial class frmMain : Form
    {
        private readonly BindingSource _bindingSource = new();
        private readonly ViewModel _viewModel = new();

        public frmMain()
        {
            InitializeComponent();

            _bindingSource.DataSource = typeof(ViewModel);

            nudNumberOfPoints.DataBindings.Add(nameof(NumericUpDown.Value), _bindingSource,
                nameof(ViewModel.NumberOfPoints), false, DataSourceUpdateMode.OnPropertyChanged);

            nudNumberOfPoints.DataBindings.Add(nameof(Enabled), _bindingSource,
                nameof(ViewModel.EnableNumberOfPoints), false, DataSourceUpdateMode.OnPropertyChanged);

            cboPointSet.DataBindings.Add(nameof(ComboBox.SelectedValue), _bindingSource,
                nameof(ViewModel.PointSet), false, DataSourceUpdateMode.OnPropertyChanged);

            cboResultKind.DataBindings.Add(nameof(ComboBox.SelectedValue), _bindingSource,
                nameof(ViewModel.ResultKind), false, DataSourceUpdateMode.OnPropertyChanged);

            cboPointSet.FillByEnum<PointSet>();
            cboResultKind.FillByEnum<ResultKind>();
            _bindingSource.DataSource = _viewModel;

            btnNext.Click += (s, e) => _viewModel.DrawNext(pnlCanvas);
            _bindingSource.CurrentItemChanged += (s, e) => _viewModel.DrawNext(pnlCanvas);
        }

        private void pnlCanvas_Resize(object sender, EventArgs e)
        {
            _viewModel.DrawNext(pnlCanvas, doGeneratePoints: false);
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            _viewModel.DrawNext(pnlCanvas);
        }
    }
}
