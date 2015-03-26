clientManager = class()

local clients = {}

function clientManager:__init()
	if (File.Exists(config.files.clients)) then
		local json = File.ReadAllText(config.files.clients)
		clients = JsonConvert.Deserialize(json)
	end
end

function clientManager:explore(dir)
	print(dir)
	local files = Directory.GetFiles(dir)
	for i = 0, files.Length - 1 do
		local info = FileVersionInfo.GetVersionInfo(files[i])
		if info.FileDescription == 'Tibia Player' then
			print(FileInfo(files[i]).FullName)
			pcall(function()
				local version = info.FileVersion
				version = version:replace('%.', '')
				if #version < 3 then
					version = version .. '0'
				elseif #version > 3 then
					version = version:remove(-1, 1)
				end
				version = version:insert(-2, '.')
				print(version)
			end)
		end
	end
	
	local dirs = Directory.GetDirectories(dir)
	for i = 0, dirs.Length - 1 do
		self:explore(dirs[i])
	end
end
