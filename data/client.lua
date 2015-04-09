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
	self.proc:Close()
	self.proc:WaitForExit()
end

function client:setHost(ip, port, rsa)
	local mem = Memory(self.proc)
	local addr = mem:FindPattern(hex('04000000'), 'xxxxxxxxxxxxxxxx', Encoding.UTF8:GetBytes('login01.tibia.com'))
	print(addr:ToString())
	print("done")
	--write(mem, hex('0058E408'), 42)
	mem:Dispose()
end
