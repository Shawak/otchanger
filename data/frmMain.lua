frmMain = class()

local function btnApplyClick(sender, e)
	MessageBox.Show("Apply!")
end

function frmMain:__init()
	self.form = Form()
	self.form:SuspendLayout()

	self.cboxHost = ComboBox()
	self.cboxHost.FormattingEnabled = true
	self.cboxHost.Location = Point(12, 12)
	self.cboxHost.Size = Size(150, 21)
	self.cboxHost.Text = "127.0.0.1"
	self.cboxHost.TabIndex = 0

	self.nudPort = NumericUpDown()
	self.nudPort.Location = Point(168, 13)
	self.nudPort.Size = Size(70, 20)
	self.nudPort.Minimum = 0
	self.nudPort.Maximum = 65555
	self.nudPort.Text = "7171"
	self.nudPort.TabIndex = 1

	self.cboxProtocol = ComboBox()
	self.cboxProtocol.FormattingEnabled = true
	self.cboxProtocol.Location = Point(244, 12)
	self.cboxProtocol.Size = Size(60, 21)
	self.cboxProtocol.DropDownStyle = ComboBoxStyle.DropDownList;
	self.cboxProtocol.TabIndex = 2

	self.btnApply = Button()
	self.btnApply.Location = Point(12, 40)
	self.btnApply.Size = Size(226, 23)
	self.btnApply.TabIndex = 3
	self.btnApply.Text = "Apply"
	self.btnApply.UseVisualStyleBackColor = true
	self.btnApply.Click:Add(btnApplyClick)

	self.btnSettings = Button()
	self.btnSettings.Location = Point(244, 40)
	self.btnSettings.Size = Size(60, 23)
	self.btnSettings.TabIndex = 4
	self.btnSettings.Text = "Settings"
	self.btnSettings.UseVisualStyleBackColor = true

	self.form.AutoScaleDimensions = SizeF(6, 13);
	self.form.AutoScaleMode = AutoScaleMode.Font;
	self.form.ClientSize = Size(316, 75)
	self.form.FormBorderStyle = FormBorderStyle.FixedSingle;
	self.form.MaximizeBox = false
	self.form.Text = "otchanger"
	self.form.StartPosition = FormStartPosition.CenterScreen
	self.form.FormClosing:Add(function() otchanger.exit() end)

	self.form.Controls:Add(self.cboxHost)
	self.form.Controls:Add(self.nudPort)
	self.form.Controls:Add(self.cboxProtocol)
	self.form.Controls:Add(self.btnApply)
	self.form.Controls:Add(self.btnSettings)

	self.form:ResumeLayout(false)
end

function frmMain:Show()
	self.form:Show()
	self.btnApply:Select()
end
