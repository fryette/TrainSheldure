/// <binding BeforeBuild='build' Clean='clean' />
var gulp = require('gulp');
var concat = require('gulp-concat');
var sequence = require('run-sequence');
var del = require('del');
var bs = require('browser-sync');
var templateCache = require('gulp-angular-templatecache');
var minifyHTML = require('gulp-minify-html');
var streamqueue = require('streamqueue');
var path = require('path');

var wwwroot = '../Trains.Web/wwwroot/';

var livereload = bs.create();

gulp.task('default', function (done) {
  sequence('clean', 'build', done);
});

gulp.task('clean', function (done) {
  del([path.join(wwwroot, '**/*.js'), path.join(wwwroot, '**/*.css')], { force: true }, done);
});

gulp.task('build', ['scripts', 'styles', 'images', 'fonts', 'index.html']);

gulp.task('index.html', function () {
  return gulp.src('src/index.html')
      .pipe(gulp.dest(wwwroot));
});

gulp.task('styles', function () {
  return gulp.src([    
    'src/**/*.css'
  ]).pipe(concat('auto-bundle.css'))
    .pipe(gulp.dest(wwwroot));
});

gulp.task('scripts', function () {
  var bower = gulp.src([
    'bower_components/jquery/dist/jquery.min.js',    
    'bower_components/angular/angular.min.js',
    'bower_components/angular-ui-router/release/angular-ui-router.min.js',
  ]);

  var templates = gulp.src('src/**/*.html')
      .pipe(minifyHTML({ empty: true, quotes: true }))
      .pipe(templateCache({
        standalone: true
      }));

  var src = gulp.src([
    'src/app.js',    
    'src/**/*.js'
  ]);

  return streamqueue.obj(bower, templates, src).pipe(concat('auto-bundle.js'))
    .pipe(gulp.dest(wwwroot));
});

gulp.task('images', function () {
  return gulp.src([    
    'src/images/*'
  ]).pipe(gulp.dest(path.join(wwwroot, 'images')));
});

gulp.task('fonts', ['main-fonts'], function () {
  return gulp.src([    
  ]).pipe(gulp.dest(wwwroot));
});

gulp.task('main-fonts', function () {
  return gulp.src([    
  ]).pipe(gulp.dest(path.join(wwwroot, 'fonts')));
});

gulp.task('browser-sync', function (done) {
  livereload.init({
    open: false,
    port: 2001,
    server: {
      baseDir: wwwroot
    },
    files: [path.join(wwwroot, '**/*'), '!' + path.join(wwwroot, 'App_Data/**/*'), '!' + path.join(wwwroot, 'web.config')]
  }, done);
});

gulp.task('serve', function () {
  sequence('build', 'browser-sync', function () {
    gulp.watch('src/**/*.js', ['scripts']);
    gulp.watch('src/**/*.css', ['styles']);
  });
});
