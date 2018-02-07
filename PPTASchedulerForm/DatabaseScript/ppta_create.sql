CREATE DATABASE ppta;

USE ppta;

GRANT ALL ON ppta.* TO ppta@localhost IDENTIFIED BY 'ppta2017';

CREATE TABLE Teacher (
   teacher_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
   code VARCHAR(5) NOT NULL UNIQUE,
   last_name VARCHAR(30),
   first_name VARCHAR(30),
   middle_name VARCHAR(30),
   email VARCHAR(100),
  
   PRIMARY KEY(teacher_id)
) ENGINE=InnoDB;

CREATE TABLE Judge_Availability (
  judge_avail_id INTEGER UNSIGNED PRIMARY KEY AUTO_INCREMENT,
  teacher_id INTEGER UNSIGNED NOT NULL UNIQUE,
  judging_level VARCHAR(5) NOT NULL,
  availability_start TIME NOT NULL,
  availability_end TIME NOT NULL,
  
  FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id)
)ENGINE=InnoDB;

ALTER TABLE Judge_Availability ADD INDEX idx_Judge_Availability_level_start_end (judging_level, availability_start, availability_end);

CREATE TABLE Student (
  student_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  last_name VARCHAR(30) NOT NULL,
  first_name VARCHAR(30) NOT NULL,
  middle_name VARCHAR(30),
  email VARCHAR(100),
  teacher_id INTEGER UNSIGNED NOT NULL,

  PRIMARY KEY(student_id),
  FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id),
  CONSTRAINT unk_student_last_name_first_name UNIQUE (last_name,first_name)
) ENGINE=InnoDB;

CREATE TABLE StudentPerformance (
  performance_id INTEGER UNSIGNED PRIMARY KEY AUTO_INCREMENT,
  student_id INTEGER UNSIGNED NOT NULL UNIQUE,
  theory VARCHAR(5),
  performance VARCHAR(5),
  preferred_start TIME,
  preferred_end TIME,
  FOREIGN KEY (student_id) REFERENCES Student(student_id)
)ENGINE=InnoDB;

ALTER TABLE StudentPerformance ADD INDEX idx_studentperformance_theory_performance (theory, performance);
ALTER TABLE StudentPerformance ADD INDEX idx_studentperformance_performance(performance);
ALTER TABLE StudentPerformance ADD INDEX idx_studentperformance_preferred_time(preferred_start, preferred_end);

CREATE TABLE Room (
   room_id VARCHAR(5),
   PRIMARY KEY(room_id)
)ENGINE=InnoDB;

CREATE TABLE Judge_break (
 judge_lunch_id INTEGER UNSIGNED PRIMARY KEY AUTO_INCREMENT,
 judge_id INTEGER UNSIGNED NOT NULL,
 break_time_start TIME NOT NULL,
 break_time_end TIME NOT NULL,
 FOREIGN KEY (judge_id) REFERENCES Teacher(teacher_id),
 CONSTRAINT unk_judge_id_break_time UNIQUE (judge_id, break_time_start, break_time_end)
)ENGINE=InnoDB;

CREATE TABLE Performance_Schedule (
 schedule_id INTEGER UNSIGNED PRIMARY KEY AUTO_INCREMENT,
 teacher_id INTEGER UNSIGNED NOT NULL, 
 student_id INTEGER UNSIGNED NOT NULL,
 start_time TIME NOT NULL,
 end_time TIME NOT NULL,
  
 FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id),
 FOREIGN KEY (student_id) REFERENCES Student(student_id),
  
 CONSTRAINT unk_schedule UNIQUE (teacher_id,start_time, end_time)
 
)ENGINE=InnoDB;

CREATE TABLE Judge_Room (
 judge_room_id INTEGER UNSIGNED AUTO_INCREMENT,
 teacher_id INTEGER UNSIGNED NOT NULL,
 room_id VARCHAR(5) NOT NULL,
 FOREIGN KEY (teacher_id) REFERENCES Teacher(teacher_id),
 FOREIGN KEY (room_id) REFERENCES Room(room_id),
 PRIMARY KEY(judge_room_id),
 CONSTRAINT unk_judge_room_teacher_id_room_id UNIQUE (teacher_id,room_id)
)ENGINE=InnoDB;

INSERT INTO Room Values ('2A'),('2B'),('2C'),('2D'),('2E'),('2F'),('2J'),('2L'),('2N'),('2S'),('2T'),('2U'),('2V'),('2W'),('2X'),('2Y'),('2Z'),('201'),('205'),('207'),('211'),('220');

