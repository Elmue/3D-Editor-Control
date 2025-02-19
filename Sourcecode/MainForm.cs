
/*****************************************************************************

This class has been written by Elmü (elmue@gmx.de)

Check if you have the latest version on:
https://www.codeproject.com/Articles/5293980/Editor3D-A-Windows-Forms-Render-Control-in-Csharp

*****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

// enums
using eRaster             = Plot3D.Editor3D.eRaster;
using ePolygonMode        = Plot3D.Editor3D.ePolygonMode;
using eSelEvent           = Plot3D.Editor3D.eSelEvent;
using eSelType            = Plot3D.Editor3D.eSelType;
using eObjType            = Plot3D.Editor3D.eObjType;
using eTooltip            = Plot3D.Editor3D.eTooltip;
using eNormalize          = Plot3D.Editor3D.eNormalize;
using eMouseCtrl          = Plot3D.Editor3D.eMouseCtrl;
using eScatterShape       = Plot3D.Editor3D.eScatterShape;
using eColorScheme        = Plot3D.Editor3D.eColorScheme;
using eInvalidate         = Plot3D.Editor3D.eInvalidate;
using eLegendPos          = Plot3D.Editor3D.eLegendPos;
// classes
using cObject3D           = Plot3D.Editor3D.cObject3D;
using cPoint3D            = Plot3D.Editor3D.cPoint3D;
using cShape3D            = Plot3D.Editor3D.cShape3D;
using cLine3D             = Plot3D.Editor3D.cLine3D;
using cPolygon3D          = Plot3D.Editor3D.cPolygon3D;
using cColorScheme        = Plot3D.Editor3D.cColorScheme;
using cMessgData          = Plot3D.Editor3D.cMessgData;
using cSurfaceData        = Plot3D.Editor3D.cSurfaceData;
using cScatterData        = Plot3D.Editor3D.cScatterData;
using cLineData           = Plot3D.Editor3D.cLineData;
using cPolygonData        = Plot3D.Editor3D.cPolygonData;
using cUserInput          = Plot3D.Editor3D.cUserInput;
// callback function
using delRendererFunction = Plot3D.Editor3D.delRendererFunction;

namespace Plot3D
{
    public partial class MainForm : Form
    {
        #region enums

        enum eDemo
        {
            Math_Callback,
            Math_Formula,
            Surface_Fill,
            Surface_Grid,
            Surface_Fill_Missing,
            Surface_Grid_Missing,
            Nested_Graphs,
            Scatter_Plot,
            Connected_Lines,
            Scatter_Shapes,
            Pyramid,
            Sphere_Fill_Closed,
            Sphere_Fill_Open,
            Sphere_Grid,
            Valentine,
            Animation,       
        }

        #endregion

        eDemo        me_Demo;
        eColorScheme me_ColorScheme;
        Timer        mi_StatusTimer = new Timer();
        cMessgData   mi_MesgTop     = new cMessgData("", -7,  7, Color.Blue); // For special hint
        cMessgData   mi_MesgBottom  = new cMessgData("", -7, -7, Color.Gray); // For selection mode

        // Only for demo "Animation"
        Timer        mi_AnimationTimer = new Timer();
        cScatterData mi_SinusData;
        cPoint3D[]   mi_Pyramid;
        int          ms32_AnimationAngle;

        public MainForm()
        {
            InitializeComponent();

            mi_StatusTimer.Interval = 8000;
            mi_StatusTimer.Tick += new EventHandler(OnClearStatusTimer);

            mi_AnimationTimer.Interval = 100;
            mi_AnimationTimer.Tick += new EventHandler(OnAnimationTimer);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // This is optional. If you don't want to use Trackbars leave this away.
            editor3D.AssignTrackBars(trackRho, trackTheta, trackPhi);

            // This is only required for DemoSphere() where the DEL key deletes polygons
            editor3D.KeyDown += new KeyEventHandler(OnEditorKeyDown);

            cMessgData i_Mesg = new cMessgData("Please select a 3D Demo in the combobox at the left", 10, 10, Color.Blue);

            editor3D.Clear();
            editor3D.AddMessageData(i_Mesg);
            editor3D.Invalidate();

            comboDemo.Sorted = false;
            foreach (eDemo e_Demo in Enum.GetValues(typeof(eDemo)))
            {
                comboDemo.Items.Add(e_Demo.ToString().Replace('_', ' '));
            }

            comboColors.Sorted = false;
            foreach (eColorScheme e_Scheme in Enum.GetValues(typeof(eColorScheme)))
            {
                comboColors.Items.Add(e_Scheme.ToString().Replace('_', ' '));
            }
            comboColors.SelectedIndex = (int)eColorScheme.Rainbow_Bright;

            comboRaster.Sorted = false;
            foreach (eRaster e_Raster in Enum.GetValues(typeof(eRaster)))
            {
                comboRaster.Items.Add(e_Raster);
            }
            comboRaster.SelectedIndex = (int)eRaster.Labels;

            comboDemo .SelectedIndex = -1; // set empty
            comboMouse.SelectedIndex = 0;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // ToolStripStatusLabel.AutoSize works like SHIT: If the window is too small, the text will not be displayed at all!
            statusLabel.AutoSize = false;
            statusLabel.Width = ClientSize.Width - 30;
        }

        /// <summary>
        /// Restore the default message in the status bar
        /// </summary>
        void OnClearStatusTimer(object sender, EventArgs e)
        {
            mi_StatusTimer.Stop();
            statusLabel.Text = "";
        }

        // ================================================================================================

        private void comboMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboMouse.SelectedIndex)
            {
                case 0:
                    editor3D.SetUserInputs(eMouseCtrl.L_Theta_R_Phi);
                    labelMouseInfo.Text = "Left mouse: Elevate,  Right mouse: Rotate";
                    break;
                case 1:
                    editor3D.SetUserInputs(eMouseCtrl.L_Theta_L_Phi);
                    labelMouseInfo.Text = "Left mouse: Elevate and Rotate";
                    break;
                case 2:
                    editor3D.SetUserInputs(eMouseCtrl.M_Theta_M_Phi);
                    labelMouseInfo.Text = "Middle mouse: Elevate and Rotate";
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            labelMouseInfo.Text += ",  Left mouse + SHIFT: Move,  Left mouse + CTRL or wheel: Zoom, Left mouse + ALT: Select";
        }

        private void comboDemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawDemo();
        }

        private void comboColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawDemo();
        }

        private void checkMirrorX_CheckedChanged(object sender, EventArgs e)
        {
            editor3D.AxisX.Mirror = checkMirrorX.Checked;
            editor3D.Invalidate();
        }

        private void checkMirrorY_CheckedChanged(object sender, EventArgs e)
        {
            editor3D.AxisY.Mirror = checkMirrorY.Checked;
            editor3D.Invalidate();
        }

        private void checkIncludeZeroZ_CheckedChanged(object sender, EventArgs e)
        {
            editor3D.AxisZ.IncludeZero = checkIncludeZeroZ.Checked;
            editor3D.Invalidate();
        }

        private void checkPointSelection_CheckedChanged(object sender, EventArgs e)
        {
            editor3D.Selection.SinglePoints = checkPointSelection.Checked;
            SetSelectionMessages();
            editor3D.Invalidate();
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            editor3D.Selection.DeSelectAll();
            editor3D.Invalidate();
        }

        private void comboRaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            editor3D.Raster = (eRaster)comboRaster.SelectedIndex;
            editor3D.Invalidate();

            checkIncludeZeroZ.Enabled = comboRaster.SelectedIndex != (int)eRaster.Off;
            if (!checkIncludeZeroZ.Enabled)
                 checkIncludeZeroZ.Checked = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            editor3D.SetCoefficients(1350, 70, 230);
            editor3D.Invalidate();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            mi_StatusTimer.Stop();
            if (editor3D.UndoBuffer.Undo())
            {
                statusLabel.Text = "";
            }
            else
            {
                statusLabel.Text = "Undo not possible";
                mi_StatusTimer.Start();
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            mi_StatusTimer.Stop();
            if (editor3D.UndoBuffer.Redo())
            {
                statusLabel.Text = "";
            }
            else
            {
                statusLabel.Text = "Redo not possible";
                mi_StatusTimer.Start();
            }
        }

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            SaveFileDialog i_Dlg = new SaveFileDialog();
            i_Dlg.Title      = "Save as PNG image";
            i_Dlg.Filter     = "PNG Image|*.png";
            i_Dlg.DefaultExt = ".png";

            if (DialogResult.Cancel == i_Dlg.ShowDialog(this))
                return;

            Bitmap i_Bitmap = editor3D.GetScreenshot();
            try
            {
                i_Bitmap.Save(i_Dlg.FileName, ImageFormat.Png);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================================================

        private void DrawDemo()
        {
            checkPointSelection.Enabled = true; // Some of the demos will disable this checkbox
            comboColors        .Enabled = true; // Some of the demos will disable this combobox

            mi_AnimationTimer.Stop();
            mi_StatusTimer   .Stop();
            me_Demo              = (eDemo)       comboDemo.SelectedIndex;
            me_ColorScheme       = (eColorScheme)comboColors.SelectedIndex;     
            editor3D.TooltipMode = eTooltip.All;

            switch (me_Demo)
            {
                case eDemo.Math_Callback:        DemoCallback();                         break;
                case eDemo.Math_Formula:         DemoFormula();                          break;
                case eDemo.Surface_Fill:         DemoSurface(ePolygonMode.Fill,  false); break;
                case eDemo.Surface_Grid:         DemoSurface(ePolygonMode.Lines, false); break;
                case eDemo.Surface_Fill_Missing: DemoSurface(ePolygonMode.Fill,  true);  break;
                case eDemo.Surface_Grid_Missing: DemoSurface(ePolygonMode.Lines, true);  break;
                case eDemo.Nested_Graphs:        DemoNestedGraphs();                     break;
                case eDemo.Scatter_Plot:         DemoScatterPlot(false);                 break;
                case eDemo.Connected_Lines:      DemoScatterPlot(true);                  break;
                case eDemo.Scatter_Shapes:       DemoScatterShapes();                    break;
                case eDemo.Sphere_Fill_Closed:   DemoSphere(ePolygonMode.Fill,  true);   break;
                case eDemo.Sphere_Fill_Open:     DemoSphere(ePolygonMode.Fill,  false);  break;
                case eDemo.Sphere_Grid:          DemoSphere(ePolygonMode.Lines, true);   break;
                case eDemo.Valentine:            DemoValentine();                        break;
                case eDemo.Pyramid:              DemoPyramid();                          break;
                case eDemo.Animation:            DemoAnimation();                        break;
                default: return;
            }

            // A demo may have changed the axis mode --> adapt checkboxes
            checkMirrorX     .Checked = editor3D.AxisX.Mirror;
            checkMirrorY     .Checked = editor3D.AxisY.Mirror;
            checkIncludeZeroZ.Checked = editor3D.AxisZ.IncludeZero;

            btnUndo.Enabled = editor3D.UndoBuffer.Enabled;
            btnRedo.Enabled = editor3D.UndoBuffer.Enabled;

            btnDeselect.Enabled = editor3D.Selection.Enabled;

            // All demos call editor3D.Clear() --> messages must be added always anew.
            editor3D.AddMessageData(mi_MesgTop, mi_MesgBottom);

            // Show total count of Lines, Shapes, Polygons
            lblInfo.Text = editor3D.ObjectStatistics;

            statusLabel.Text = (editor3D.Selection.Callback == null) ? "Callback: OFF" : "";

            SetSelectionMessages();
        }

        void SetSelectionMessages()
        {
            mi_MesgTop   .Text = "";
            mi_MesgBottom.Text = "";

            if (editor3D.Selection.Enabled)
            {
                if (me_Demo == eDemo.Sphere_Fill_Closed || me_Demo == eDemo.Sphere_Fill_Open)
                {
                    mi_MesgTop.Text = "DEL key: Delete selected polygons, CTRL+Z: Undo, CTRL+Y: Redo";
                }
                else if (editor3D.Selection.Callback != null)
                {
                    String s_Obj = "points";
                    if (!editor3D.Selection.SinglePoints)
                    {
                        switch (me_Demo)
                        {
                            case eDemo.Surface_Fill:    s_Obj = "polygons"; break;
                            case eDemo.Scatter_Shapes:
                            case eDemo.Scatter_Plot:    s_Obj = "shapes"; break;
                            case eDemo.Pyramid:
                            case eDemo.Connected_Lines: s_Obj = "lines"; break;
                        }
                    }
                    mi_MesgTop.Text = "ALT + CTRL + left mouse: Move selected "+s_Obj+", CTRL+Z: Undo, CTRL+Y: Redo";
                }

                String s_Multi = editor3D.Selection.MultiSelect  ? "ON" : "OFF";
                mi_MesgBottom.Text      = "Multiple selection: "+s_Multi+", Selection color: ▇▇▇▇▇▇";
                mi_MesgBottom.TextColor = editor3D.Selection.HighlightColor;
            }
            else
            {
                checkPointSelection.Enabled = false;
                checkPointSelection.Checked = false;

                mi_MesgBottom.Text      = "Selection is disabled";
                mi_MesgBottom.TextColor = Color.Gray;
            }
        }

        // ================================================================================================

        /// <summary>
        /// This demonstrates how to use a mathematical callback function which calculates Z values from X and Y
        /// </summary>
        private void DemoCallback()
        {
            cColorScheme i_Scheme = new cColorScheme(me_ColorScheme);
            cSurfaceData i_Data   = new cSurfaceData(49, 33, ePolygonMode.Fill, Pens.Black, i_Scheme);

            delRendererFunction f_Callback = delegate(double X, double Y)
            {
                double r = 0.15 * Math.Sqrt(X * X + Y * Y);
                if (r < 1e-10) return 120;
                else           return 120 * Math.Sin(r) / r;
            };

            i_Data.ExecuteFunction(f_Callback, new PointF(-120, -80), new PointF(120, 80));

            cMessgData i_Mesg1 = new cMessgData("r = 0.15 * sqrt(x * x + y * y)", 7, -24, Color.Indigo);
            cMessgData i_Mesg2 = new cMessgData("z = 120  * sin(r) / r",          7,  -4, Color.Indigo);

            // IMPORTANT: Normalize maintainig the relation between X,Y,Z values otherwise the function will be distorted.
            editor3D.Clear();
            editor3D.Normalize = eNormalize.MaintainXYZ;
            editor3D.AddMessageData(i_Mesg1, i_Mesg2);
            editor3D.AddRenderData (i_Data);

            editor3D.Selection.Callback = null;  
            editor3D.Selection.Enabled  = false;
            editor3D.UndoBuffer.Enabled = false;
            editor3D.Invalidate();

            // Selection does not make sense for this demo
        }

        // ================================================================================================

        /// <summary>
        /// This demonstrates how to use a string formula which calculates Z values from X and Y
        /// </summary>
        private void DemoFormula()
        {
            cColorScheme i_Scheme = new cColorScheme(me_ColorScheme);
            cSurfaceData i_Data   = new cSurfaceData(41, 41, ePolygonMode.Fill, Pens.Black, i_Scheme);

            String s_Formula = "7 * sin(x) * cos(y) / (sqrt(sqrt(x * x + y * y)) + 0.2)";

            try
            {
                delRendererFunction f_Function = FunctionCompiler.Compile(s_Formula);

                i_Data.ExecuteFunction(f_Function, new PointF(-7, -7), new PointF(7, 7));
            }
            catch (Exception Ex) // invalid formula
            {
                MessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cMessgData i_Mesg = new cMessgData("Formula: z = " + s_Formula, 7, -7, Color.Indigo);

            // IMPORTANT: Normalize maintainig the relation between X,Y,Z values otherwise the function will be distorted.
            editor3D.Clear();
            editor3D.Normalize = eNormalize.MaintainXYZ;
            editor3D.AddMessageData(i_Mesg);
            editor3D.AddRenderData (i_Data);
            
            editor3D.Selection.Callback = null;  
            editor3D.Selection.Enabled  = false;
            editor3D.UndoBuffer.Enabled = false;
            editor3D.Invalidate();

            // Selection does not make sense for this demo
        }

        // ================================================================================================

        /// <summary>
        /// This demonstrates how to set X, Y, Z values directly (without math function)
        /// </summary>
        private void DemoSurface(ePolygonMode e_Mode, bool b_Missing)
        {
            #region int s32_Values definition

            int[,] s32_Values = new int[,]
            {
                {  9059,  9634, 10617, 11141, 11838, 12681, 13411, 13861, 14121, 14624, 14868, 15172, 15368, 15368, 15368, 15368, 15368},
                {  9684, 10387, 11141, 11796, 12546, 13337, 14029, 14320, 14549, 14811, 14939, 15434, 15794, 15794, 15794, 15794, 15794},
                { 10486, 11370, 12255, 13009, 13861, 14746, 15172, 15368, 15434, 15630, 15794, 15991, 16351, 16351, 16351, 16351, 16351},
                { 11469, 12354, 13533, 14287, 15008, 15925, 16187, 16482, 16690, 16976, 17105, 17302, 17531, 17531, 17531, 17531, 17531},
                { 12452, 13435, 14615, 15499, 16253, 17105, 17596, 17924, 18153, 18285, 18428, 18776, 19104, 19104, 19104, 19104, 19104},
                { 13337, 14516, 15729, 16679, 17564, 18514, 18907, 19169, 19399, 19661, 19792, 19594, 20152, 20152, 20152, 20152, 20152},
                { 14746, 15860, 17039, 17990, 18842, 19595, 20050, 20349, 20546, 20840, 20972, 20972, 21332, 21332, 21332, 21332, 21332},
                { 16155, 17236, 18350, 19399, 20251, 20677, 21016, 21332, 21660, 21791, 21889, 21955, 22217, 22217, 22217, 22217, 22217},
                { 17334, 18579, 19497, 20775, 21463, 21848, 22288, 22446, 22643, 22446, 22643, 22708, 23069, 23069, 23069, 23069, 23069},
                { 18743, 19792, 20808, 22086, 22805, 23167, 23486, 23366, 23757, 23411, 23691, 23822, 23822, 23822, 23822, 23822, 23822},
                { 20677, 21445, 22544, 23593, 24441, 24785, 24538, 24644, 24773, 24299, 24062, 24576, 24510, 24510, 24510, 24510, 24510},
                { 23475, 23888, 24478, 25330, 26212, 26199, 25701, 25664, 25740, 25013, 24904, 25068, 25374, 25374, 25374, 25374, 25374},
                { 27525, 27518, 26701, 27334, 27682, 27402, 26903, 26707, 26444, 25887, 25719, 25690, 26122, 26122, 26122, 26122, 26122},
                { 30769, 30423, 29917, 29231, 29392, 28705, 28075, 27726, 27263, 26691, 26417, 26182, 26575, 26575, 25246, 25246, 25246},
                { 32672, 32178, 31916, 31469, 31246, 30540, 29852, 29065, 28377, 27623, 27263, 26706, 26935, 26367, 25049, 25049, 25049},
                { 33522, 33227, 32702, 32615, 32452, 31851, 30933, 30179, 29295, 28358, 27984, 27132, 27301, 26367, 25049, 25049, 25049},
                { 33980, 33587, 33161, 32935, 32588, 32342, 31621, 30802, 29852, 29000, 28377, 27689, 27421, 26367, 25049, 25049, 25049},
                { 34210, 33784, 33423, 33161, 32801, 32408, 31785, 31097, 30474, 29622, 29000, 28115, 27623, 26367, 25049, 25049, 25049},
                { 34472, 34079, 33653, 33161, 32768, 32408, 31785, 31162, 30802, 30048, 29360, 28312, 27755, 26367, 25049, 25049, 25049},
                { 34603, 34144, 33718, 33227, 32768, 32342, 31719, 30999, 31228, 30333, 29622, 28606, 27886, 26492, 25167, 25167, 25167},
                { 34669, 34210, 33653, 33096, 32539, 32047, 31490, 30933, 31293, 30671, 29983, 28803, 27886, 26492, 25167, 25167, 25167},
                { 34767, 34210, 33718, 33096, 32342, 31851, 31228, 30867, 31457, 30867, 30266, 28934, 27984, 26492, 25167, 25167, 25167},
            };

            #endregion
            int s32_Cols = s32_Values.GetLength(1);
            int s32_Rows = s32_Values.GetLength(0);

            cColorScheme i_Scheme = new cColorScheme(me_ColorScheme);

            // In Line mode the pen is used to draw the polygon border lines. The color is assigned from the ColorScheme.
            // In Fill mode the pen is used to draw the thin separator lines (always 1 pixel, black)
            Pen i_Pen = (e_Mode == ePolygonMode.Lines) ? new Pen(Color.Yellow, 2) : Pens.Black;

            // In mode 'Missing' circles with a radius of 4 pixels represent single points that have insufficient neighbours to draw a polygon.
            int s32_Radius = b_Missing ? 4 : 0;

            cSurfaceData i_Data = new cSurfaceData(s32_Cols, s32_Rows, e_Mode, i_Pen, i_Scheme, s32_Radius);

            for (int C=0; C<i_Data.Cols; C++)
            {
                for (int R=0; R<i_Data.Rows; R++)
                {
                    if (b_Missing)
                    {
                        // Skip some points which will be missing.
                        bool b_Skip = C > 4 && C < 8 && R > 9 && R < 14;
                        if (C ==  6 && R == 11) b_Skip = false;
                        if (C ==  6 && R == 12) b_Skip = false;
                        if (C ==  6 && R == 13) b_Skip = false;
                        if (C ==  8 && R == 11) b_Skip = true;
                        if (C ==  7 && R ==  4) b_Skip = true;
                        if (C ==  8 && R ==  4) b_Skip = true;
                        if (C == 16 && R ==  0) b_Skip = true;
                        if (C == 15 && R ==  0) b_Skip = true;
                        if (C == 16 && R ==  1) b_Skip = true;
                        if (C == 15 && R ==  1) b_Skip = true;
                        if (b_Skip)
                            continue;
                    }

                    int s32_RawValue = s32_Values[R,C];

                    double d_X = C * 640.0; // X must be related to Colum
                    double d_Y = R *   5.0; // Y must be related to Row
                    double d_Z = s32_RawValue / 327.68;

                    String s_Tooltip = String.Format("Speed = {0} rpm\nMAP = {1} kPa\nVolume Eff. = {2} %\nColumn = {3}\nRow = {4}", 
                                                     d_X, d_Y, Editor3D.FormatDouble(d_Z), C, R);

                    cPoint3D i_Point = new cPoint3D(d_X, d_Y, d_Z, s_Tooltip, s32_RawValue);
                    i_Data.SetPointAt(C, R, i_Point);
                }
            }

            // IMPORTANT: Normalize X,Y,Z separately because the axes have different ranges
            editor3D.Clear();
            editor3D.Normalize    = eNormalize.Separate;
            editor3D.TooltipMode  = eTooltip.UserText;
            editor3D.AxisY.Mirror = true;
            editor3D.AxisX.LegendText = "Engine Speed (rpm)";
            editor3D.AxisY.LegendText = "MAP (kPa)";
            editor3D.AxisZ.LegendText = "Volume Efficiency (%)";
            editor3D.LegendPos        = eLegendPos.BottomLeft;
            editor3D.AddRenderData(i_Data);

            editor3D.Selection.Callback       = OnSelectEvent;
            editor3D.Selection.HighlightColor = Color.FromArgb(90, 90, 90);
            editor3D.Selection.MultiSelect    = true;
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            if (e_Mode == ePolygonMode.Fill) // Polygons
            {
                i_Data.GetPolygonAt(10, 5).Selected = true;
            }
            else // Lines
            {
                // Selection of polygons is not possible here.
                checkPointSelection.Enabled = false;
                checkPointSelection.Checked = true;

                i_Data.GetPointAt(10, 5).Selected = true;
                i_Data.GetPointAt(10, 6).Selected = true;
                i_Data.GetPointAt(11, 5).Selected = true;
                i_Data.GetPointAt(11, 6).Selected = true;
            }
        }

        // ================================================================================================

        /// <summary>
        /// This loads 2 graphs, one nested into the other
        /// </summary>
        private void DemoNestedGraphs()
        {
            const int POINTS = 8;
            cSurfaceData i_Data1 = new cSurfaceData(POINTS, POINTS, ePolygonMode.Lines, new Pen(Color.Orange, 3), null);
            cSurfaceData i_Data2 = new cSurfaceData(POINTS, POINTS, ePolygonMode.Lines, new Pen(Color.Green,  2), null);

            for (int C=0; C<POINTS; C++)
            {
                for (int R=0; R<POINTS; R++)
                {
                    double d_X = (C - POINTS / 2.3) / (POINTS / 5.5); // X must be related to Colum !
                    double d_Y = (R - POINTS / 2.3) / (POINTS / 5.5); // Y must be related to Row !
                    double d_Radius = Math.Sqrt(d_X * d_X + d_Y * d_Y);
                    double d_Z = Math.Cos(d_Radius) + 1.0;

                    String  s_Tooltip = String.Format("Col = {0}\nRow = {1}", C, R);
                    cPoint3D i_Point1 = new cPoint3D(d_X, d_Y, d_Z,       s_Tooltip + "\nWrong Data");
                    cPoint3D i_Point2 = new cPoint3D(d_X, d_Y, d_Z * 0.6, s_Tooltip + "\nCorrect Data");

                    i_Data1.SetPointAt(C, R, i_Point1);
                    i_Data2.SetPointAt(C, R, i_Point2);
                }
            }

            cMessgData i_Mesg1 = new cMessgData("Graph with error data",   7,  -7, Color.Orange);
            cMessgData i_Mesg2 = new cMessgData("Graph with correct data", 7, -24, Color.Green);

            editor3D.Clear();
            editor3D.Normalize = eNormalize.MaintainXY;
            editor3D.AddRenderData (i_Data1, i_Data2);
            editor3D.AddMessageData(i_Mesg1, i_Mesg2);

            editor3D.Selection.HighlightColor = Color.Black;
            editor3D.Selection.MultiSelect    = false;
            editor3D.Selection.Callback       = null;  
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            // Single point selection works only in Fill mode
            checkPointSelection.Enabled = false;
            checkPointSelection.Checked = true;

            // This demo ignores the ColorScheme
            comboColors.Enabled = false; 
        }

        // ================================================================================================

        /// <summary>
        /// This demonstrates how to set X, Y, Z scatter plot points or lines in form of a spiral.
        /// </summary>
        private void DemoScatterPlot(bool b_Lines)
        {
            // 3 pixels for line width and for circle radius
            const int SIZE = 3;

            cColorScheme   i_Scheme    = new cColorScheme(me_ColorScheme);
            cScatterData   i_ShapeData = new cScatterData(i_Scheme);
            cLineData      i_LineData  = new cLineData   (i_Scheme);
            List<cPoint3D> i_Points    = new List<cPoint3D>();

            for (double P = -22.0; P < 22.0; P += 0.1)
            {
                double d_X = Math.Sin(P) * P;
                double d_Y = Math.Cos(P) * P;
                double d_Z = P;
                if (d_Z > 0.0) d_Z /= 3.0;

                cPoint3D i_Point = new cPoint3D(d_X, d_Y, d_Z, "Scatter Point");
                if (b_Lines) 
                {
                    i_Points.Add(i_Point);
                }
                else // Shapes
                {
                    // You can store the returned shape in a variable and later modify it's properties
                    cShape3D i_Shape = i_ShapeData.AddShape(i_Point, eScatterShape.Circle, SIZE, null); 
                }
            }
            
            // You can store the returned lines in a variable and later modify their properties
            cLine3D[] i_Lines = i_LineData.AddConnectedLines(i_Points, SIZE, null);

            // Depending on your use case you can also specify MaintainXY or MaintainXYZ here
            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;
            editor3D.AddRenderData(i_ShapeData, i_LineData);

            editor3D.Selection.HighlightColor = Color.FromArgb(90,90,90);
            editor3D.Selection.Callback       = OnSelectEvent;
            editor3D.Selection.MultiSelect    = true;
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            // For shapes this setting does not make a difference
            checkPointSelection.Checked = false;
            if (!b_Lines)
                checkPointSelection.Enabled = false;
        }

        // ================================================================================================

        private void DemoScatterShapes()
        {
            #region double d_Values definitions

            double[,] d_Values = new double[,]
            {
                // Value  X        Y      Z
                {   1.46, 0.0007,  0.077, 0.72 },
                {  -1.85, 0.0137,  0.053, 0.87 },
                {   5.51, 0.0047,  0.016, 1.12 },
                {   1.15, 0.0076,  0.117, 1.36 },
                {   1.98, 0.0157, -0.004, 1.23 },
                {  -2.22, 0.0029,  0.037, 1.09 },
                {   4.70, 0.0333, -0.154, 1.38 },
                {  -6.42, 0.0594, -0.228, 2.48 },
                {  -7.93, 0.0487, -0.394, 1.24 },
                {   1.57, 0.0874, -0.504, 0.78 },
                {  -6.92, 0.0739, -0.395, 1.05 },
                {   4.65, 0.0341, -0.484, 2.18 },
                {   7.10, 0.0326, -0.477, 2.00 },
                {   3.31, 0.0024, -0.090, 0.62 },
                {   6.83, 0.0138, -0.045, 1.04 },
                {   3.71, 0.0137,  0.033, 0.81 },
                {   1.95, 0.0043,  0.147, 0.89 },
                {   4.91, 0.0192,  0.046, 1.69 },
                {  -7.47, 0.0488, -0.021, 1.18 },
                {  -1.09, 0.1051, -0.221, 1.17 },
                {  -1.72, 0.0322, -0.244, 0.95 },
                {   1.83, 0.0078,  0.083, 1.12 },
                {   1.71, 0.0049,  0.080, 0.79 },
                {  -7.24, 0.0012,  0.077, 2.08 },
                {   6.08, 0.0644, -0.131, 1.28 },
                {   1.86, 0.0131,  0.088, 0.69 },
                {   2.80, 0.0010,  0.068, 1.03 },
                {   1.66, 0.0094,  0.158, 1.20 },
                {   1.34, 0.0106,  0.162, 1.06 },
                {   2.36, 0.0090,  0.016, 1.18 },
                {   4.98, 0.0204,  0.118, 1.36 },
                {   3.02, 0.0314,  0.042, 1.57 },
                {  -7.98, 0.0452, -0.069, 1.06 },
                {   3.45, 0.0900, -0.390, 1.49 },
                {  -6.74, 0.0270, -0.688, 1.64 },
                { -12.86, 0.0538, -0.283, 1.87 },
                {  -9.34, 0.0526, -0.671, 1.56 },
                {  10.03, 0.0389, -0.981, 1.49 },
                {   5.26, 0.0299, -0.463, 1.31 },
                {   8.95, 0.0248, -0.442, 0.78 },
                {   5.51, 0.0182, -0.007, 0.90 },
                {   1.94, 0.0060,  0.356, 0.59 },
                {   1.23, 0.0041,  0.260, 0.97 },
                {  14.45, 0.0526, -0.013, 1.40 },
                {  -7.35, 0.0467,  0.223, 1.45 },
                {  -7.39, 0.0479, -0.138, 0.76 },
                {   2.00, 0.0174, -0.406, 1.05 },
                {   1.70, 0.0159, -0.080, 0.95 },
                {   1.74, 0.0073,  0.060, 0.51 },
                {   7.04, 0.0567, -0.400, 0.97 },
                {   1.20, 0.0077,  0.195, 0.98 },
                {   4.47, 0.0043,  0.206, 0.76 },
                {   3.85, 0.0297, -0.106, 0.99 },
                {   3.75, 0.0372, -0.085, 1.51 },
                {  -7.03, 0.0149,  0.077, 0.58 },
                {  -3.14, 0.0625, -0.537, 1.06 },
                {   4.01, 0.0421, -0.884, 1.34 },
                {   2.83, 0.0164, -0.375, 1.60 },
                {  -1.09, 0.0118, -0.143, 0.83 },
                {   2.59, 0.0291, -0.264, 0.78 },
                {   1.31, 0.0136,  0.581, 0.65 },
                {   4.08, 0.0142,  0.321, 0.65 },
                {   3.77, 0.0084,  0.219, 0.97 },
                {  -2.02, 0.0253, -0.548, 0.68 },
                {  -3.00, 0.0204, -0.658, 1.18 },
                {  -7.95, 0.0095, -0.283, 1.33 },
                {   3.54, 0.0592, -0.752, 1.35 },
                {   3.91, 0.0872, -1.002, 1.14 },
                {  -1.11, 0.0040, -0.305, 0.91 },
                { -11.04, 0.0265, -0.409, 0.93 },
                {   3.27, 0.0689, -1.163, 1.56 },
                {  -6.89, 0.0663, -0.678, 2.17 },
                {   1.12, 0.0448, -0.321, 1.40 },
                {   3.26, 0.0076,  0.161, 0.80 },
                {   2.00, 0.0056,  0.334, 0.63 },
                {  -2.28, 0.0138,  0.373, 0.92 },
                {  -2.61, 0.0264,  0.446, 0.88 },
                {  -9.24, 0.0299, -0.309, 0.79 },
            };
            #endregion

            // A ColorScheme is not needed because all points have their own Brush
            cScatterData i_Data = new cScatterData(null);

            for (int P = 0; P < d_Values.GetLength(0); P++)
            {
                double d_Value = d_Values[P, 0];
                int s32_Radius = (int)Math.Abs(d_Value) + 1;

                double X = d_Values[P,1];
                double Y = d_Values[P,2];
                double Z = d_Values[P,3];

                eScatterShape e_Shape = (d_Value < 0) ? eScatterShape.Square : eScatterShape.Triangle;
                Brush         i_Brush = (d_Value < 0) ? Brushes.Red          : Brushes.Lime;

                String s_Tooltip = "Value = " + Editor3D.FormatDouble(d_Value);

                // The original double value is passed to the callback when the user selects this point with ALT + Left click.
                cPoint3D i_Point = new cPoint3D(X, Y, Z, s_Tooltip, d_Value);  
                
                // You can store the returned shape in a variable and later modify it's properties
                cShape3D i_Shape = i_Data.AddShape(i_Point, e_Shape, s32_Radius, i_Brush);

                // pre-select the biggest shapes
                if (Math.Abs(d_Value) > 10)
                    i_Shape.Selected = true;
            }

            cMessgData i_Mesg1 = new cMessgData("Negative Values (size of square represents value)",   7,  -7, Color.Red);
            cMessgData i_Mesg2 = new cMessgData("Positive Values (size of triangle represents value)", 7, -24, Color.Lime);

            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;
            editor3D.AddRenderData (i_Data);
            editor3D.AddMessageData(i_Mesg1, i_Mesg2);

            editor3D.Selection.HighlightColor = Color.Blue;
            editor3D.Selection.Callback       = OnSelectEvent;
            editor3D.Selection.MultiSelect    = true;
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            // For scatter shapes Single Point selection mode does not make a difference
            checkPointSelection.Enabled = false;

            // This demo ignores the ColorScheme
            comboColors.Enabled = false; 
        }

        // ================================================================================================

        /// <summary>
        /// This demonstrates how to set X, Y, Z scatterplot points in form of a heart
        /// </summary>
        private void DemoValentine()
        {
            const int WIDTH = 12;
            List<cPoint3D> i_Points = new List<cPoint3D>();

            double X = 0.0;
            double Z = 0.0;
            for (double P = 0.0; P <= Math.PI * 1.32; P += 0.025)
            {
                X = Math.Cos(P) * 1.8 - 1.8;
                Z = Math.Sin(P) * 3.0 + 6.0;

                i_Points.Add   (   new cPoint3D( X, -X, Z, "Upper Right Part"));
                i_Points.Insert(0, new cPoint3D(-X,  X, Z, "Upper Left Part"));
            }

            double d_X = X / 70;
            double d_Z = Z / 70;
            while (Z >= 0.0)
            {
                i_Points.Add   (   new cPoint3D( X, -X, Z, "Lower Right Part"));
                i_Points.Insert(0, new cPoint3D(-X,  X, Z, "Lower Left Part"));

                X -= d_X;
                Z -= d_Z;
            }

            i_Points.Add(new cPoint3D(0.0, 0.0, 0.0, "Zero"));

            cLineData i_Data = new cLineData(new cColorScheme(Color.Red));
            i_Data.AddConnectedLines(i_Points, WIDTH, null);

            cMessgData i_Mesg = new cMessgData("Happy Valentine's day, Sweetheart!", 7, -7, Color.Red);

            editor3D.Clear();
            editor3D.Normalize = eNormalize.MaintainXYZ;
            editor3D.AddRenderData (i_Data);
            editor3D.AddMessageData(i_Mesg);

            editor3D.Selection.Callback = null;  
            editor3D.Selection.Enabled  = false; 
            editor3D.UndoBuffer.Enabled = false;
            editor3D.Invalidate();

            // Selection does not make sense for this demo

            // This demo ignores the ColorScheme
            comboColors.Enabled = false; 
        }

        // ================================================================================================

        private void DemoSphere(ePolygonMode e_Mode, bool b_Closed)
        {
            const int LONGI  = 50; // count of 3D points along the longitude (360 degree)
            const int LATI   = 25; // count of 3D points along the latitude  (180 degree)
            const int RADIUS = 20;

            // ------ Calculate 3D points ------ 

            cPoint3D[,] i_Points = new cPoint3D[LONGI, LATI];

            for (int Long=0; Long<LONGI; Long++)
            {
                double d_Theta = 2 * Math.PI / LONGI * Long;

                for (int Lati=0; Lati<LATI; Lati++)
                {
                    double d_Phi = Math.PI / LATI * Lati;

                    // Cartesian coordinates
                    double X = RADIUS * Math.Sin(d_Phi) * Math.Cos(d_Theta) * 1.3;
                    double Y = RADIUS * Math.Sin(d_Phi) * Math.Sin(d_Theta);
                    double Z = RADIUS * Math.Cos(d_Phi);

                    i_Points[Long, Lati] = new cPoint3D(X, Y, Z);
                }
            }

            // ------ Create rectangular 3D polygons ------ 

            // In Line mode the pen is used to draw the polygon border lines. The color is assigned from the ColorScheme.
            // In Fill mode the pen is used to draw the thin separator lines (always 1 pixel, black)
            Pen i_Pen = (e_Mode == ePolygonMode.Lines) ? new Pen(Color.Yellow, 2) : Pens.Black;

            cColorScheme i_Scheme = new cColorScheme(me_ColorScheme);
            cPolygonData i_Data   = new cPolygonData(e_Mode, i_Pen, i_Scheme);

            // Omit top and bottom where ploygons become very narrow.
            // In case of the open sphere, omit 4 rows of rectangles so the interior becomes visible.
            int s32_Omit = b_Closed ? 1 : 4;

            for (int Long=0; Long<LONGI; Long++) // 0 .... 360 degree
            {
                // overflow to zero if maximum exceeded
                int NextLong = (Long + 1) % LONGI; 

                for (int Lati=s32_Omit; Lati<LATI-s32_Omit; Lati++) 
                {
                    int NextLati = Lati + 1;

                    // You can store the returned polygon in a variable and later modify it's properties
                    cPolygon3D i_Poly = i_Data.AddPolygon(null, i_Points[Long,     Lati],
                                                                i_Points[NextLong, Lati],
                                                                i_Points[NextLong, NextLati],
                                                                i_Points[Long,     NextLati]);                   
                }
            }

            // ----- Close the top and bottom with a round polygon ------

            if (e_Mode == ePolygonMode.Fill && b_Closed)
            {
                List<cPoint3D> i_ListTop    = new List<cPoint3D>();
                List<cPoint3D> i_ListBottom = new List<cPoint3D>();
                for (int Long=0; Long<LONGI; Long++) // 0 .... 360 degree
                {
                    i_ListTop   .Add(i_Points[Long, 1]);
                    i_ListBottom.Add(i_Points[Long, LATI-1]);
                }

                // You can store the returned polygon in a variable and later modify it's properties
                cPolygon3D i_PolyTop    = i_Data.AddPolygon(null, i_ListTop   .ToArray());
                cPolygon3D i_PolyBottom = i_Data.AddPolygon(null, i_ListBottom.ToArray());
            }

            editor3D.Clear();
            editor3D.Normalize = eNormalize.MaintainXYZ;
            editor3D.AxisY.LegendText = "Longitude";
            editor3D.AxisZ.LegendText = "Latitude";
            editor3D.LegendPos        = eLegendPos.AxisEnd;
            editor3D.AddRenderData(i_Data);

            editor3D.Selection.HighlightColor = Color.FromArgb(90,90,90);
            editor3D.Selection.Callback       = null;  
            editor3D.Selection.MultiSelect    = true;
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            // Single point selection works only in Fill mode
            if (e_Mode == ePolygonMode.Lines)
                checkPointSelection.Enabled = false;

            // FIRST: Adjust Selection.SinglePoints which will remove all selections
            checkPointSelection.Checked = (e_Mode == ePolygonMode.Lines);

            // AFTER: Pre-select one polygon
            cPolygon3D i_Polygon = i_Data.AllPolygons[5];
            if (editor3D.Selection.SinglePoints)
            {
                // Select the 4 corner points of the polygon
                foreach (cPoint3D i_Point in i_Polygon.Points)
                {
                    i_Point.Selected = true;
                }
            }
            else
            {
                // Select the polygon itself
                i_Polygon.Selected = true;
            }

            // Send key events to the editor
            editor3D.Focus();
        }

        /// <summary>
        /// This event handler will only receive key events while the Editor3D control has the keyboard focus!
        /// If Editor3D does not show the blue border you must click into the editor.
        /// </summary>
        void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            if (me_Demo != eDemo.Sphere_Fill_Closed &&
                me_Demo != eDemo.Sphere_Fill_Open)
                return;

            // Delete all selected polygons
            cObject3D[] i_Selected = editor3D.Selection.GetSelectedObjects(eSelType.Polygon);
            if (i_Selected.Length > 0)
            {
                editor3D.RemoveObjects(i_Selected);

                mi_StatusTimer.Stop();
                statusLabel.Text = i_Selected.Length + " polygons have been removed.";
                mi_StatusTimer.Start();
            }

            // update statistics
            lblInfo.Text = editor3D.ObjectStatistics;

            editor3D.Invalidate();
        }

        // ================================================================================================

        private void DemoPyramid()
        {
            const int COLOR_PARTS = 50;
            cLineData i_Data = new cLineData(new cColorScheme(me_ColorScheme));

            cPoint3D i_Center  = new cPoint3D(45, 45, 40, "Center");
            cPoint3D i_Corner1 = new cPoint3D(45, 25, 20, "Corner 1");
            cPoint3D i_Corner2 = new cPoint3D(25, 45, 20, "Corner 2");
            cPoint3D i_Corner3 = new cPoint3D(45, 65, 20, "Corner 3");
            cPoint3D i_Corner4 = new cPoint3D(65, 45, 20, "Corner 4");

            // Add the 4 vertical lines which are rendered as 50 parts with different colors
            // You can store the returned line in a variable and later modify it's properties
            cLine3D i_Vert1 = i_Data.AddMultiColorLine(COLOR_PARTS, i_Center, i_Corner1, 4, null);
            cLine3D i_Vert2 = i_Data.AddMultiColorLine(COLOR_PARTS, i_Center, i_Corner2, 4, null);
            cLine3D i_Vert3 = i_Data.AddMultiColorLine(COLOR_PARTS, i_Center, i_Corner3, 4, null);
            cLine3D i_Vert4 = i_Data.AddMultiColorLine(COLOR_PARTS, i_Center, i_Corner4, 4, null);

            // Add the 4 base lines with solid color
            // You can store the returned line in a variable and later modify it's properties
            cLine3D i_Hor1 = i_Data.AddSolidLine(i_Corner1, i_Corner2, 8, null);
            cLine3D i_Hor2 = i_Data.AddSolidLine(i_Corner2, i_Corner3, 8, null);
            cLine3D i_Hor3 = i_Data.AddSolidLine(i_Corner3, i_Corner4, 8, null);
            cLine3D i_Hor4 = i_Data.AddSolidLine(i_Corner4, i_Corner1, 8, null);

            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;
            editor3D.AxisZ.IncludeZero = false;
            editor3D.AddRenderData(i_Data);

            editor3D.Selection.HighlightColor = Color.Green;
            editor3D.Selection.Callback       = OnSelectEvent;
            editor3D.Selection.MultiSelect    = true;
            editor3D.Selection.Enabled        = true;
            editor3D.UndoBuffer.Enabled       = true;
            editor3D.Invalidate();

            // default: select lines of outer pyramid
            checkPointSelection.Checked = false;
        }

        // ================================================================================================

        void DemoAnimation()
        {
            const int SHAPES = 50;

            // The animation needs ColorScheme RainbowSweep which provides a cyclic rainbow with all colors.
            // The other schemes are useless because they have an incomplete rainbow or only 64 colors.
                       mi_SinusData = new cScatterData(new cColorScheme(eColorScheme.Rainbow_Sweep));
            cPolygonData i_PolyData = new cPolygonData(ePolygonMode.Fill, Pens.Black, null);

            for (int i=0; i<SHAPES; i++)
            {
                // The coordinates of the points will be set in ProcessAnimation()
                // You can store the returned shape in a variable and later modify it's properties
                cShape3D i_Shape = mi_SinusData.AddShape(new cPoint3D(0,0,0), eScatterShape.Circle, 5, null);
            }

            // ------------------------------------------------------

            // store the points of the pyramid
            mi_Pyramid    = new cPoint3D[5];
            mi_Pyramid[0] = new cPoint3D(-100, -100, 75, "Top");
            mi_Pyramid[1] = new cPoint3D(-100,  -50, 50, "Edge 1");
            mi_Pyramid[2] = new cPoint3D( -50, -100, 50, "Edge 2");
            mi_Pyramid[3] = new cPoint3D(-100, -150, 50, "Edge 3");
            mi_Pyramid[4] = new cPoint3D(-150, -100, 50, "Edge 4");

            // Create the polygons
            cPolygon3D i_Poly1 = i_PolyData.AddPolygon(Brushes.Orange,    mi_Pyramid[0], mi_Pyramid[1], mi_Pyramid[2]); // Side
            cPolygon3D i_Poly2 = i_PolyData.AddPolygon(Brushes.Gold,      mi_Pyramid[0], mi_Pyramid[2], mi_Pyramid[3]); // Side
            cPolygon3D i_Poly3 = i_PolyData.AddPolygon(Brushes.Goldenrod, mi_Pyramid[0], mi_Pyramid[3], mi_Pyramid[4]); // Side
            cPolygon3D i_Poly4 = i_PolyData.AddPolygon(Brushes.Sienna,    mi_Pyramid[0], mi_Pyramid[4], mi_Pyramid[1]); // Side
            cPolygon3D i_Poly5 = i_PolyData.AddPolygon(Brushes.Tomato,    mi_Pyramid[1], mi_Pyramid[2], mi_Pyramid[3], mi_Pyramid[4]); // Bottom

            ProcessAnimation();

            // ------------------------------------------------------

            cMessgData i_Mesg = new cMessgData("CPU Load < 1%", 7, -7, Color.Gray);

            editor3D.Clear();
            editor3D.Normalize = eNormalize.Separate;
            editor3D.AddRenderData (mi_SinusData, i_PolyData);
            editor3D.AddMessageData(i_Mesg);

            editor3D.Selection.Callback = null;              
            editor3D.Selection.Enabled  = false; 
            editor3D.UndoBuffer.Enabled = false;
            editor3D.Invalidate();

            mi_AnimationTimer.Start();

            // Selection does not make sense for this demo

            // This demo ignores the ColorScheme
            comboColors.Enabled = false; 
        }

        void OnAnimationTimer(object sender, EventArgs e)
        {
            ProcessAnimation();
            editor3D.Invalidate();
        }

        void ProcessAnimation()
        {
            ms32_AnimationAngle ++;

            // ======== SCATTER =========

            cShape3D[]   i_AllShapes   = mi_SinusData.AllShapes;
            cColorScheme i_ColorScheme = mi_SinusData.ColorScheme;
            double       d_DeltaX      = 400.0 / i_AllShapes.Length;

            double d_X = -200.0;
            for (int S=0; S<i_AllShapes.Length; S++, d_X += d_DeltaX)
            {
                cShape3D i_Shape = i_AllShapes[S];

                i_Shape.Points[0].X =  d_X;
                i_Shape.Points[0].Y = -d_X;
                i_Shape.Points[0].Z = Math.Sin((ms32_AnimationAngle + d_X) / 50.0) * 50.0 + 50.0;

                i_Shape.Brush = i_ColorScheme.GetBrush(ms32_AnimationAngle * 10);
            }

            // ======== PYRAMID =========

            double d_Angle   = ms32_AnimationAngle / 30.0;
            double d_Sinus   = Math.Sin(d_Angle) * 50.0; // -50 ... +50
            double d_Cosinus = Math.Cos(d_Angle) * 50.0; // -50 ... +50
            double d_DeltaZ  = d_Sinus / 2.0;            // -25 ... +25

            // Top
            mi_Pyramid[0].X = -100.0;
            mi_Pyramid[0].Y = -100.0;
            mi_Pyramid[0].Z =   70.0 + d_DeltaZ; 
            // Edge 1
            mi_Pyramid[1].X = -100.0 + d_Sinus;
            mi_Pyramid[1].Y = -100.0 + d_Cosinus;
            mi_Pyramid[1].Z =   40.0 + d_DeltaZ;
            // Edge 2
            mi_Pyramid[2].X = -100.0 + d_Cosinus;
            mi_Pyramid[2].Y = -100.0 - d_Sinus;
            mi_Pyramid[2].Z =   40.0 + d_DeltaZ;
            // Edge 3
            mi_Pyramid[3].X = -100.0 - d_Sinus;
            mi_Pyramid[3].Y = -100.0 - d_Cosinus;
            mi_Pyramid[3].Z =   40.0 + d_DeltaZ;
            // Edge 4
            mi_Pyramid[4].X = -100.0 - d_Cosinus;
            mi_Pyramid[4].Y = -100.0 + d_Sinus;
            mi_Pyramid[4].Z =   40.0 + d_DeltaZ;
        }

        // ================================================================================================

        /// <summary>
        /// This callback is used by multiple demos.
        /// The callback function is called when the left mouse is down and the ALT key is pressed.
        /// Select objects with ALT only. Drag with ALT + CTRL.
        /// e_Event is the current mouse action (Down, Drag, Up).
        /// e_Modifiers are the modifier keys that are down (Control, Shift, Alt).
        /// s32_DeltaX, s32_DeltaY are the relative mouse movement in pixels since the last event.
        /// i_Object is the 3D object (point, shape, line, polygon) that the mouse is clicking or dragging.
        /// ATTENTION: i_Object may be null if the user has ALT-clicked a location without 3D object!
        /// Read the detailed comment of function Editor3D.SelectionCallback()
        /// </summary>
        private eInvalidate OnSelectEvent(eSelEvent e_Event, Keys e_Modifiers, int s32_DeltaX, int s32_DeltaY, cObject3D i_Object)
        {
            eInvalidate e_Invalidate = eInvalidate.NoChange;

            bool b_CTRL = (e_Modifiers & Keys.Control) > 0;
            
            // The left mouse button went down with ALT key down and CTRL key up
            if (e_Event == eSelEvent.MouseDown && !b_CTRL && i_Object != null)
            {
                i_Object.Selected = !i_Object.Selected;

                // After changing the selection status the object must be redrawn.
                e_Invalidate = eInvalidate.Invalidate;
            }
            else if (e_Event == eSelEvent.MouseDrag && b_CTRL)
            {
                // The user is dragging the mouse with ALT + CTRL keys down.
                // Convert the mouse movement in the 2D space into a movement in the 3D space.
                cPoint3D i_Project = editor3D.ReverseProject(s32_DeltaX, s32_DeltaY);

                // GetSelectedPoints() returns only unique points.
                cPoint3D[] i_Selected = editor3D.Selection.GetSelectedPoints(eSelType.All);
                foreach (cPoint3D i_Point in i_Selected)
                {
                    switch (me_Demo)
                    {
                        case eDemo.Pyramid:
                        case eDemo.Scatter_Shapes:
                        case eDemo.Scatter_Plot:
                        case eDemo.Connected_Lines:
                            // The pyramid line end points / scatter shapes can be moved freely in the 3D space
                            i_Point.Move(i_Project.X, i_Project.Y, i_Project.Z);
                            break;

                        case eDemo.Surface_Fill:
                        case eDemo.Surface_Grid:
                        case eDemo.Surface_Fill_Missing:
                        case eDemo.Surface_Grid_Missing:
                            // The points in the Surface grid have a fixed X,Y position, only Z can be modified.
                            i_Point.Move(0, 0, i_Project.Z);
                            break;

                        default:
                            Debug.Assert(false);
                            break;
                    }
                }

                // Set flag to recalculate the coordinate system, then Invalidate()
                e_Invalidate = eInvalidate.CoordSystem;
            }

            mi_StatusTimer.Stop();
            statusLabel.Text = String.Format("Callback Event: {0},        Modifiers: {1},        DeltaX: {2},        DeltaY: {3},        Object: {4}", 
                                             e_Event, e_Modifiers, s32_DeltaX, s32_DeltaY, 
                                             (i_Object == null) ? "null" : i_Object.ObjType.ToString());
            mi_StatusTimer.Start();
            return e_Invalidate;
        }
    }
}
