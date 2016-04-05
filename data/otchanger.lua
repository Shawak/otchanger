import('System')
import('System.IO')
import('System.Text')
import('System.Drawing')
import('System.Diagnostics')
import('System.Windows.Forms')
import('System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
import('ShawLib')

local manager

otchanger = {}
otchanger.init = function()
	--print('otchanger\nLua Version: ' .. _VERSION .. '\n\n')

	-- load constants
	dofile('config.lua')

	-- laod libs
	dofile('libs/core.lua')
	dofile('libs/string.lua')
	dofile('libs/class.lua')

	-- show available lua functions
	dofile('scripts/luafunctions.lua')
	-- show referenced assemblies
	File.WriteAllText("assemblies.txt", exportAssemblies())

	-- load project files
	dofile('client.lua')
	dofile('clientManager.lua')
	dofile('frmMain.lua')

	if (config.console) then
		showConsole()
	end

	-- create missing dirs to be able to create files
	for k, v in pairs({config.dirs.appdata, config.dirs.settings}) do
		if not Directory.Exists(v) then
			Directory.CreateDirectory(v)
		end
	end

	manager = clientManager()
	manager:load()
	manager:explore(config.dirs.programmsX86 .. 'Tibia/')
	
	local client = manager:start('10.70')
	client:setHost(nil, nil, nil)
	
	frmMain():Show()
end

otchanger.exit = function()
	if config.closeClientsOnShutdown then
		manager:shutdown()
	end

	manager:save()
	exit()
end

otchanger.init()
