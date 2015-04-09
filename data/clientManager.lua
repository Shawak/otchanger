clientManager = class()

function clientManager:__init()
	self.clients = {}
	self.procs = {}
end

function clientManager:load()
	if not File.Exists(config.files.clients) then
		return
	end
end

function clientManager:save()

end

function clientManager:start(version)
	local fileInfo = self.clients[version]
	if not fileInfo then
		return nil
	end

	local client = client(fileInfo)
	client:start()
	self.procs[#self.procs + 1] = client
	return client
end

function clientManager:explore(dir)
	if not Directory.Exists(dir) then
		return 0
	end

	local found = 0
	local files = Directory.GetFiles(dir)
	for i = 0, files.Length - 1 do
		local info = FileVersionInfo.GetVersionInfo(files[i])
		if info.FileDescription == 'Tibia Player' and info.LegalCopyright:contains('CipSoft') then
			local version = info.FileVersion
			version = version:replace('%.', '')
			if #version < 3 then
				version = version .. '0'
			elseif #version > 3 then
				version = version:remove(-1, 1)
			end
			version = version:insert(-2, '.')
			print('client version ' .. version .. ' found in ' .. dir)
			self.clients[version] = FileInfo(files[i])
			found = found + 1
		end
	end
	
	local dirs = Directory.GetDirectories(dir)
	for i = 0, dirs.Length - 1 do
		found = found + self:explore(dirs[i])
	end

	return found
end

function clientManager:shutdown()
	for i = 1, #self.procs do
		self.procs[i]:close()
	end
end
