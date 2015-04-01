client = class()

function client:__init(fileInfo)
	self.file = fileInfo.FullName
	self.dir = fileInfo.Directory.FullName
end

function client:start()
	local info = ProcessStartInfo()
	info.FileName = self.file
	info.WorkingDirectory = self.dir
	local proc = Process()
	proc.StartInfo = info
	proc:Start()
	proc:WaitForInputIdle()
end

function client:stop()
	proc:Close()
	proc:WaitForExit()
end

function client:setHost(ip, port, rsa)

end
