import('System')
import('System.IO')
import('System.Text')
import('System.Drawing')
import('System.Diagnostics')
import('System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
import('System.Windows.Forms')
import('Newtonsoft.Json')

config = {
	console = true,
}

config.dirs = {}
config.dirs.appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
config.dirs.settings = config.dirs['appdata'] .. '/otchanger/settings/'

config.dirs.programmsX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) .. '/'

config.files = {}
config.files.clients = config.dirs.settings .. 'clients.json'

otchanger = {}
otchanger.init = function()
	if (config.console) then
		dofile('console.lua')
		Console.Show()
	end
	print('Running ' .. _VERSION)

	-- create missing dirs to be able to create files
	for k, v in pairs({config.dirs.appdata, config.dirs.settings}) do
		if (not Directory.Exists(v)) then
			Directory.CreateDirectory(v)
		end
	end

	File.WriteAllText("assemblies.txt", exportAssemblies())
	
	-- laod libs
	dofile('libs/string.lua')
	dofile('libs/class.lua')

	-- show available lua functions
	dofile('scripts/luafunctions.lua')

	-- load project files
	dofile('console.lua')
	dofile('clientManager.lua')
	dofile('frmMain.lua')

	-- load needed assemblies/types
	--[[luanet.load_assembly('System.Windows.Forms')
	Forms = luanet.System.Windows.Forms
	Application = Forms.Application -- luanet.import_type('System.Windows.Forms.Application')
	Form = Forms.Form --luanet.import_type('System.Windows.Forms.Form')]]

	local manager = clientManager()
	manager:explore(config.dirs.programmsX86 .. 'Tibia/')
	
	frmMain():Show()
end

otchanger.exit = function()
	exit()
end

otchanger.init()
