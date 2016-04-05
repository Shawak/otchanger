
config = {
	console = true,
	closeClientsOnShutdown = true
}

config.dirs = {}
config.dirs.appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) .. '/'
config.dirs.settings = config.dirs.appdata .. 'otchanger/settings/'
config.dirs.programmsX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) .. '/'

config.files = {}
config.files.clients = config.dirs.settings .. 'clients.json'


-- taken from otclient
OTSERV_RSA  = "1091201329673994292788609605089955415282375029027981291234687579" ..
              "3726629149257644633073969600111060390723088861007265581882535850" ..
              "3429057592827629436413108566029093628212635953836686562675849720" ..
              "6207862794310902180176810615217550567108238764764442605581471797" ..
              "07119674283982419152118103759076030616683978566631413"

CIPSOFT_RSA = "1321277432058722840622950990822933849527763264961655079678763618" ..
              "4334395343554449668205332383339435179772895415509701210392836078" ..
              "6959821132214473291575712138800495033169914814069637740318278150" ..
              "2907336840325241747827401343576296990629870233111328210165697754" ..
              "88792221429527047321331896351555606801473202394175817"