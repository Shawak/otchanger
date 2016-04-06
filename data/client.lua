client = class()

function client:__init(fileInfo)
	self.ipAddresses = {}
	self.portAddresses = {}

	self.file = fileInfo.FullName
	self.dir = fileInfo.Directory.FullName
	self.proc = nil
end

function client:getMemory()
	return Memory(self.proc.Id)
end

function client:start()
	local info = ProcessStartInfo()
	info.FileName = self.file
	info.WorkingDirectory = self.dir

	local proc = Process()
	proc.StartInfo = info
	proc:Start()
	proc:WaitForInputIdle()
	self.proc = proc

	self:exploreAddresses()
end

function client:close()
	NativeMethods.TerminateProcess(self.proc.Handle, 0)
end

function client:setHost(ip, port, rsa)

	--[[local mem = Memory(self.proc.Id)

	local addr = mem:Search(bytes('login01.tibia.com'))
	print(addr:ToString())
	print(readString(mem,  addr))
	writeString(mem, addr, 'shawak.de')

	addr = mem:Search(bytes(CIPSOFT_RSA))
	print(addr:ToString())

	mem:Dispose()]]

	print("___")
	local mem = self:getMemory()
	for i = 1, #self.ipAddresses do
		print(self.ipAddresses[i])-- .. ' ' .. readString(mem, self.ipAddresses[i]))
	end
	print("--")
	for i = 1, #self.portAddresses do
		print(self.portAddresses[i])
	end
	print("##")

end

function client:exploreAddresses()
	local ips = { -- string
		-- >= 8.x
		'tibia01.cipsoft.com',
		'tibia02.cipsoft.com',
		'tibia03.cipsoft.com',
		'tibia04.cipsoft.com',
		'tibia05.cipsoft.com',
		'login01.tibia.com',
		'login02.tibia.com',
		'login03.tibia.com',
		'login04.tibia.com',
		'login05.tibia.com',

		-- 7.x
		'test.cipsoft.com',
		'server.tibia.com',
		'server2.tibia.com',
		'tibia1.cipsoft.com',
		'tibia2.cipsoft.com',
	}

	local ports = { -- 8 bytes (read 8, write 2)
		-- >= 10.70
		-- 03 1c 10 00  00 00 00 00
		72707,

		-- <= 8.6
		-- 03 1c 00 00  01 00 00 00
		4294974467,
	}

	local mem = self:getMemory()
	for i = 1, #ips do
		local addresses = toTable(mem:Search(getBytes(ips[i])))
		for k = 1, #addresses do
			self.ipAddresses[#self.ipAddresses + 1] = addresses[k]
		end
	end

	--[[local addr = nil
	for i = 1, #ports do
		addr = self:searchAddress(mem, ports[i], addr)
		if addr then
			self.portAddresses[#self.portAddresses + 1] = addr
			i = i - 1
		end
	end]]

	-- TODO: FIXME
	local mem = self:getMemory()
	for i = 1, #ports do
		local addresses = toTable(mem:Search(getBytes(ports[i])))
		for k = 1, #addresses do
			self.portAddresses[#self.portAddresses + 1] = addresses[k]
		end
	end

	mem:Dispose()
end