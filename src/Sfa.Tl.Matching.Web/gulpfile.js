/// <binding BeforeBuild='default' />

var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('./gulp/paths.json')
const sassOptions = require('./gulp/sassOptions.js')

require('./gulp/tasks/dev')
require('./gulp/tasks/default')

gulp.task('default', ['govuk-js', 'copy-js', 'copy-employer-js', 'copy-assets', 'sass', 'merge-css']);
gulp.task('dev', ['dev-govuk-js', 'dev-copy-js', 'dev-copy-employer-js', 'dev-copy-assets', 'dev-sass', 'dev-merge-css']);


