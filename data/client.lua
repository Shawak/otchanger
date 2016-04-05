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

	local proc = Process()
	proc.StartInfo = info
	proc:Start()
	proc:WaitForInputIdle()
	self.proc = proc
end

function client:close()
	NativeMethods.TerminateProcess(self.proc.Handle, 0)
end

function client:setHost(ip, port, rsa)
	local mem = Memory(self.proc.Id)

	local addr = mem:FindPattern(bytes('login01.tibia.com'))
	print(addr:ToString())

	addr = mem:FindPattern(bytes(CIPSOFT_RSA))
	print(addr:ToString())

	mem:Dispose()
end
