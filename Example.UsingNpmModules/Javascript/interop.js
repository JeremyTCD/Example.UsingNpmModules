const lodash = require('lodash');

module.exports = (callback, arg) => {
    callback(null, lodash.kebabCase(arg));
}