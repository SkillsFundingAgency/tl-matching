const { src } = require('gulp');

var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('../paths.json');
const sassOptions = require('../sassOptions.js');

gulp.task('govuk-js', () => {
    return src([
        'node_modules/govuk-frontend/*.js',
        'node_modules/govuk-frontend/vendor/**.js',
        'node_modules/govuk-frontend/components/**/*.js',
    ])
        .pipe(gulp.dest('wwwroot/govuk/javascripts'));
});

gulp.task('copy-js', function () {
    return src([
        'node_modules/jquery/dist/jquery.min.js',
        'Frontend/src/javascripts/cookie-banner.js',
        'Frontend/src/javascripts/filter.js',

        ])
        .pipe(concat('all.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-opportunity-basket-js', function () {
    return src([
        'Frontend/src/javascripts/opportunity-basket.js'
    ])
        .pipe(concat('opportunity-basket.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-editquals-js', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/edit-qualifications.js'
    ])
        .pipe(concat('edit-quals.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-employer-js', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/employer-search.js'
    ])
        .pipe(concat('employer-search.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-missing-quals-js', function () {
    return src([
            'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
            'Frontend/src/javascripts/missing-qualification-search.js'
        ])
        .pipe(concat('missing-quals.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-assets', () => {
   return src(paths.src.defaultAssets)
        .pipe(gulp.dest(paths.dist.defaultAssets));
});

gulp.task('sass', () => {
    return src(paths.src.default)
        .pipe(sass(sassOptions))
        .pipe(gulp.dest(paths.mid.default));
}
);


gulp.task('merge-css', gulp.series('sass', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.css',
        "Frontend/src/stylesheets/css/*.css"
    ])
        .pipe(concatCss("main.css"))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest(paths.dist.default));
    }
));
