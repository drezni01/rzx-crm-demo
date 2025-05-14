import io.xh.hoist.configuration.LogbackConfig
import io.hoistapp.log.CustomLogSupportConverter

// Example deeply overriding format via custom converter.
LogbackConfig.monitorLayout = '%d{HH:mm:ss.SSS} | %customMsg%n'
conversionRule('customMsg', CustomLogSupportConverter)

LogbackConfig.defaultConfig(this)
