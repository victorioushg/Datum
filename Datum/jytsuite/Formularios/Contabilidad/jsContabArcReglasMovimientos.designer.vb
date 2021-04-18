<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsContabArcReglasMovimientos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsContabArcReglasMovimientos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtConcepto = New System.Windows.Forms.TextBox()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.grpConstante = New System.Windows.Forms.GroupBox()
        Me.MenuGrupo = New System.Windows.Forms.ToolStrip()
        Me.btnCamposGrupo = New System.Windows.Forms.ToolStripButton()
        Me.btnFormulaGrupo = New System.Windows.Forms.ToolStripButton()
        Me.btnConceptoGrupo = New System.Windows.Forms.ToolStripButton()
        Me.btnConstanteGrupo = New System.Windows.Forms.ToolStripButton()
        Me.txtAgrupadoPor = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MenuCondicion = New System.Windows.Forms.ToolStrip()
        Me.btnCamposC = New System.Windows.Forms.ToolStripButton()
        Me.btnFormulasC = New System.Windows.Forms.ToolStripButton()
        Me.btnConceptosC = New System.Windows.Forms.ToolStripButton()
        Me.btnConstantesC = New System.Windows.Forms.ToolStripButton()
        Me.MenuFormula = New System.Windows.Forms.ToolStrip()
        Me.btnCamposF = New System.Windows.Forms.ToolStripButton()
        Me.btnFormulasF = New System.Windows.Forms.ToolStripButton()
        Me.btnConceptosF = New System.Windows.Forms.ToolStripButton()
        Me.btnConstantesF = New System.Windows.Forms.ToolStripButton()
        Me.txtCondicion = New System.Windows.Forms.TextBox()
        Me.txtFormula = New System.Windows.Forms.TextBox()
        Me.txtConjuntoNombre = New System.Windows.Forms.TextBox()
        Me.btnconjunto = New System.Windows.Forms.Button()
        Me.txtConjunto = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.grpConstante.SuspendLayout()
        Me.MenuGrupo.SuspendLayout()
        Me.MenuCondicion.SuspendLayout()
        Me.MenuFormula.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 415)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(805, 30)
        Me.lblInfo.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Regla Nº :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 19)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Descripción :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtConcepto
        '
        Me.txtConcepto.Location = New System.Drawing.Point(136, 19)
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.Size = New System.Drawing.Size(67, 20)
        Me.txtConcepto.TabIndex = 12
        Me.txtConcepto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(136, 40)
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(657, 20)
        Me.txtDescripcion.TabIndex = 18
        '
        'grpConstante
        '
        Me.grpConstante.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpConstante.Controls.Add(Me.MenuGrupo)
        Me.grpConstante.Controls.Add(Me.txtAgrupadoPor)
        Me.grpConstante.Controls.Add(Me.Label5)
        Me.grpConstante.Controls.Add(Me.MenuCondicion)
        Me.grpConstante.Controls.Add(Me.MenuFormula)
        Me.grpConstante.Controls.Add(Me.txtCondicion)
        Me.grpConstante.Controls.Add(Me.txtFormula)
        Me.grpConstante.Controls.Add(Me.txtConjuntoNombre)
        Me.grpConstante.Controls.Add(Me.btnconjunto)
        Me.grpConstante.Controls.Add(Me.txtConjunto)
        Me.grpConstante.Controls.Add(Me.Label4)
        Me.grpConstante.Controls.Add(Me.Label3)
        Me.grpConstante.Controls.Add(Me.Label2)
        Me.grpConstante.Controls.Add(Me.txtDescripcion)
        Me.grpConstante.Controls.Add(Me.txtConcepto)
        Me.grpConstante.Controls.Add(Me.Label7)
        Me.grpConstante.Controls.Add(Me.Label1)
        Me.grpConstante.Location = New System.Drawing.Point(0, 0)
        Me.grpConstante.Name = "grpConstante"
        Me.grpConstante.Size = New System.Drawing.Size(800, 411)
        Me.grpConstante.TabIndex = 31
        Me.grpConstante.TabStop = False
        Me.grpConstante.Text = "Regla de contabilización"
        '
        'MenuGrupo
        '
        Me.MenuGrupo.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuGrupo.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuGrupo.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuGrupo.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCamposGrupo, Me.btnFormulaGrupo, Me.btnConceptoGrupo, Me.btnConstanteGrupo})
        Me.MenuGrupo.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuGrupo.Location = New System.Drawing.Point(136, 345)
        Me.MenuGrupo.Name = "MenuGrupo"
        Me.MenuGrupo.Size = New System.Drawing.Size(99, 27)
        Me.MenuGrupo.TabIndex = 113
        Me.MenuGrupo.Text = "ToolStrip1"
        '
        'btnCamposGrupo
        '
        Me.btnCamposGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCamposGrupo.Image = Global.Datum.My.Resources.Resources.campos
        Me.btnCamposGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCamposGrupo.Name = "btnCamposGrupo"
        Me.btnCamposGrupo.Size = New System.Drawing.Size(24, 24)
        '
        'btnFormulaGrupo
        '
        Me.btnFormulaGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnFormulaGrupo.Image = Global.Datum.My.Resources.Resources.Formula
        Me.btnFormulaGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFormulaGrupo.Name = "btnFormulaGrupo"
        Me.btnFormulaGrupo.Size = New System.Drawing.Size(24, 24)
        '
        'btnConceptoGrupo
        '
        Me.btnConceptoGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConceptoGrupo.Image = Global.Datum.My.Resources.Resources.Conceptos
        Me.btnConceptoGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConceptoGrupo.Name = "btnConceptoGrupo"
        Me.btnConceptoGrupo.Size = New System.Drawing.Size(24, 24)
        '
        'btnConstanteGrupo
        '
        Me.btnConstanteGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConstanteGrupo.Image = Global.Datum.My.Resources.Resources.Constantes
        Me.btnConstanteGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConstanteGrupo.Name = "btnConstanteGrupo"
        Me.btnConstanteGrupo.Size = New System.Drawing.Size(24, 24)
        '
        'txtAgrupadoPor
        '
        Me.txtAgrupadoPor.Location = New System.Drawing.Point(136, 375)
        Me.txtAgrupadoPor.Name = "txtAgrupadoPor"
        Me.txtAgrupadoPor.Size = New System.Drawing.Size(657, 20)
        Me.txtAgrupadoPor.TabIndex = 112
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 375)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 19)
        Me.Label5.TabIndex = 111
        Me.Label5.Text = "Agrupado por ... :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MenuCondicion
        '
        Me.MenuCondicion.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuCondicion.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuCondicion.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuCondicion.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCamposC, Me.btnFormulasC, Me.btnConceptosC, Me.btnConstantesC})
        Me.MenuCondicion.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuCondicion.Location = New System.Drawing.Point(136, 225)
        Me.MenuCondicion.Name = "MenuCondicion"
        Me.MenuCondicion.Size = New System.Drawing.Size(25, 110)
        Me.MenuCondicion.TabIndex = 110
        Me.MenuCondicion.Text = "MenuCondicion"
        '
        'btnCamposC
        '
        Me.btnCamposC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCamposC.Image = Global.Datum.My.Resources.Resources.campos
        Me.btnCamposC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCamposC.Name = "btnCamposC"
        Me.btnCamposC.Size = New System.Drawing.Size(23, 24)
        '
        'btnFormulasC
        '
        Me.btnFormulasC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnFormulasC.Image = Global.Datum.My.Resources.Resources.Formula
        Me.btnFormulasC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFormulasC.Name = "btnFormulasC"
        Me.btnFormulasC.Size = New System.Drawing.Size(23, 24)
        '
        'btnConceptosC
        '
        Me.btnConceptosC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConceptosC.Image = Global.Datum.My.Resources.Resources.Conceptos
        Me.btnConceptosC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConceptosC.Name = "btnConceptosC"
        Me.btnConceptosC.Size = New System.Drawing.Size(23, 24)
        '
        'btnConstantesC
        '
        Me.btnConstantesC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConstantesC.Image = Global.Datum.My.Resources.Resources.Constantes
        Me.btnConstantesC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConstantesC.Name = "btnConstantesC"
        Me.btnConstantesC.Size = New System.Drawing.Size(23, 24)
        '
        'MenuFormula
        '
        Me.MenuFormula.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuFormula.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuFormula.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuFormula.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCamposF, Me.btnFormulasF, Me.btnConceptosF, Me.btnConstantesF})
        Me.MenuFormula.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuFormula.Location = New System.Drawing.Point(136, 112)
        Me.MenuFormula.Name = "MenuFormula"
        Me.MenuFormula.Size = New System.Drawing.Size(25, 110)
        Me.MenuFormula.TabIndex = 109
        Me.MenuFormula.Text = "ToolStrip1"
        '
        'btnCamposF
        '
        Me.btnCamposF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCamposF.Image = Global.Datum.My.Resources.Resources.campos
        Me.btnCamposF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCamposF.Name = "btnCamposF"
        Me.btnCamposF.Size = New System.Drawing.Size(23, 24)
        '
        'btnFormulasF
        '
        Me.btnFormulasF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnFormulasF.Image = Global.Datum.My.Resources.Resources.Formula
        Me.btnFormulasF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFormulasF.Name = "btnFormulasF"
        Me.btnFormulasF.Size = New System.Drawing.Size(23, 24)
        '
        'btnConceptosF
        '
        Me.btnConceptosF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConceptosF.Image = Global.Datum.My.Resources.Resources.Conceptos
        Me.btnConceptosF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConceptosF.Name = "btnConceptosF"
        Me.btnConceptosF.Size = New System.Drawing.Size(23, 24)
        '
        'btnConstantesF
        '
        Me.btnConstantesF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnConstantesF.Image = Global.Datum.My.Resources.Resources.Constantes
        Me.btnConstantesF.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConstantesF.Name = "btnConstantesF"
        Me.btnConstantesF.Size = New System.Drawing.Size(23, 24)
        '
        'txtCondicion
        '
        Me.txtCondicion.Location = New System.Drawing.Point(164, 228)
        Me.txtCondicion.Multiline = True
        Me.txtCondicion.Name = "txtCondicion"
        Me.txtCondicion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCondicion.Size = New System.Drawing.Size(629, 107)
        Me.txtCondicion.TabIndex = 97
        '
        'txtFormula
        '
        Me.txtFormula.Location = New System.Drawing.Point(164, 112)
        Me.txtFormula.Multiline = True
        Me.txtFormula.Name = "txtFormula"
        Me.txtFormula.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFormula.Size = New System.Drawing.Size(629, 110)
        Me.txtFormula.TabIndex = 96
        '
        'txtConjuntoNombre
        '
        Me.txtConjuntoNombre.Location = New System.Drawing.Point(240, 83)
        Me.txtConjuntoNombre.Name = "txtConjuntoNombre"
        Me.txtConjuntoNombre.Size = New System.Drawing.Size(553, 20)
        Me.txtConjuntoNombre.TabIndex = 95
        '
        'btnconjunto
        '
        Me.btnconjunto.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnconjunto.Location = New System.Drawing.Point(209, 83)
        Me.btnconjunto.Name = "btnconjunto"
        Me.btnconjunto.Size = New System.Drawing.Size(25, 20)
        Me.btnconjunto.TabIndex = 94
        Me.btnconjunto.Text = "•••"
        Me.btnconjunto.UseVisualStyleBackColor = True
        '
        'txtConjunto
        '
        Me.txtConjunto.Location = New System.Drawing.Point(136, 83)
        Me.txtConjunto.Name = "txtConjunto"
        Me.txtConjunto.Size = New System.Drawing.Size(67, 20)
        Me.txtConjunto.TabIndex = 29
        Me.txtConjunto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 228)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 19)
        Me.Label4.TabIndex = 28
        Me.Label4.Text = "Donde ... :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 19)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Asignar ... :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Conjunto :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(639, 415)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 88
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.Datum.My.Resources.Resources.button_cancel
        Me.btnCancel.Location = New System.Drawing.Point(85, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.Image = Global.Datum.My.Resources.Resources.button_ok
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        '
        'jsContabArcReglasMovimientos
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(805, 445)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpConstante)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsContabArcReglasMovimientos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimientos reglas de contabilización"
        Me.grpConstante.ResumeLayout(False)
        Me.grpConstante.PerformLayout()
        Me.MenuGrupo.ResumeLayout(False)
        Me.MenuGrupo.PerformLayout()
        Me.MenuCondicion.ResumeLayout(False)
        Me.MenuCondicion.PerformLayout()
        Me.MenuFormula.ResumeLayout(False)
        Me.MenuFormula.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtConcepto As System.Windows.Forms.TextBox
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents grpConstante As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtConjunto As System.Windows.Forms.TextBox
    Friend WithEvents txtCondicion As System.Windows.Forms.TextBox
    Friend WithEvents txtFormula As System.Windows.Forms.TextBox
    Friend WithEvents txtConjuntoNombre As System.Windows.Forms.TextBox
    Friend WithEvents btnconjunto As System.Windows.Forms.Button
    Friend WithEvents MenuFormula As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCamposF As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnFormulasF As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConceptosF As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConstantesF As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuCondicion As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCamposC As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnFormulasC As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConceptosC As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConstantesC As System.Windows.Forms.ToolStripButton
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MenuGrupo As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCamposGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnFormulaGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConceptoGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnConstanteGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtAgrupadoPor As System.Windows.Forms.TextBox
End Class
