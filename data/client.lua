client = class()

function client:__init(fileInfo)
	self.file = fileInfo.FullName
	self.dir = fileInfo.Directory.FullName
	self.proc = nil
end

function client:start()
	local info = ProcessStartInfo()
	info.FileName = self.file
	info.WorkingDirectory = self.dir

	self.proc = Process()
	self.proc.StartInfo = info
	self.proc:Start()
	self.proc:WaitForInputIdle()
end

function client:close()
	pcall(function()
		self.proc:Close()
		self.proc:WaitForExit()
	end)
end

function client:setHost(ip, port, rsa)
	local mem = Memory(self.proc)

	local addr = mem:FindPattern(bytes('login01.tibia.com'))
	print(addr:ToString())

	addr = mem:FindPattern(bytes(CIPSOFT_RSA))
	print(addr:ToString())

	mem:Dispose()
end
