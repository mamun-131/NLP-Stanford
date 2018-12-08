const $ = require('shelljs');
require('../logging')

$.config.fatal = true


try {
    console.warn('ToDo')
} catch (e) {
    console.error(`❎ AN UNEXPECTED ERROR OCURRED`)
    console.error(e)
    process.exit(1)
}