/// <binding BeforeBuild='default' />

var gulp = require('gulp');

require('./gulp/tasks/dev')
require('./gulp/tasks/default')

gulp.task('default', ['govuk-js', 'copy-js', 'copy-employer-js', 'copy-assets', 'merge-css']);
gulp.task('dev', ['govuk-js', 'copy-js', 'dev-copy-employer-js', 'copy-assets', 'merge-css']);


