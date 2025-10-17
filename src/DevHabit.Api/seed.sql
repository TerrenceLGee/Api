DELETE FROM dev_habit.habits;

INSERT INTO dev_habit.habits (
    id, name, description, type,
    frequency_type, frequency_times_per_period,
    status, is_archived, created_at_utc
) VALUES (
             'habit_1', 'Read for 15 minutes', 'Read any book for at least 15 minutes a day.', 1, 
             1, 1,
             1, false, NOW()
         );


INSERT INTO dev_habit.habits (
    id, name, description, type,
    frequency_type, frequency_times_per_period,
    target_value, target_unit,
    status, is_archived, created_at_utc
) VALUES (
             'habit_2', 'Run', 'Go for a run.', 2,
             2, 3,
             5, 'km',
             1, false, NOW()
         );


INSERT INTO dev_habit.habits (
    id, name, type,
    frequency_type, frequency_times_per_period,
    status, is_archived, created_at_utc
) VALUES (
             'habit_3', 'Review budget', 1,
             3, 1,
             2, true, NOW()
         );