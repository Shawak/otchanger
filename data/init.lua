import('System.Windows.Forms')
import("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
import("System.Drawing")

otchanger = {
	config = {
		console = true
	}
}

otchanger.init = function()
	if (otchanger.config.console) then
		dofile('data/console.lua')
		Console.Show()
	end
	print("Running " .. _VERSION)

	-- laod libs
	dofile('data/libs/string.lua')
	dofile('data/libs/class.lua')

	-- show available lua functions
	dofile('data/scripts/luafunctions.lua')

	-- load project files
	dofile('data/console.lua')
	dofile('data/frmMain.lua')

	-- load needed assemblies/types
	--[[luanet.load_assembly('System.Windows.Forms')
	Forms = luanet.System.Windows.Forms
	Application = Forms.Application -- luanet.import_type('System.Windows.Forms.Application')
	Form = Forms.Form --luanet.import_type('System.Windows.Forms.Form')]]

	dofile('data/frmMain.lua')
	frmMain():Show()
end

otchanger.exit = function()
	exit()
end

otchanger.init()
