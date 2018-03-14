'''
    @Author: Gabriel Rodriguez
    Date: 3/13/2018
    Project: TransformTasks

    Transform tasks was created to handle JSON objects
    JSON files are parsed to this program using the commandline args
    the JSON object is the processed and the output is a JSON file
    with objects mapping the different values present.
'''

import sys
import json
import os

# get command line args
pipe = sys.argv[1]

# dictionary for storing json object
app_dict = {}

# values to be processed
search_values = {"application", "version", "role", "location"}


# open and read parsed file and search key/values
# requires search_value as argument using loop to process all data
def read_json(find_key):
    global app_dict
    global pipe

    try:
        with open(pipe, encoding='utf-8') as data_file:
            data = json.loads(data_file.read())
            for key in data.keys():
                json_data = data[key]
                assert isinstance(json_data, object)
                for value in json_data:
                    if find_key in value:
                        app = data[key][find_key]
                        # key = nodes, app = values from keys( search_values ), find_key = search_values
                        organize(app, key, find_key)
    except IOError:
        print('An error occurred trying to read the file.')


# group data using arguments from read_json to write to dictionary
def organize(app, key, find_key):
    global app_dict

    if find_key in app_dict:
        if app in app_dict:
            # append the new number to the existing array at this slot
            app_dict[app].append(key)
        else:
            # create a new array in this slot
            app_dict[app] = [key]
    else:
        # create a new array in this slot
        app_dict[find_key] = '{'


# initialize definitions and script
def _init():
    global search_values
    to_file = "outfile.json"
    mode = ""

    # loop to send args to first def
    for k in search_values:
        read_json(k)

    # after dictionary is full write to outfile
    # TODO allow user to optionally add a source for file to write to
    try:
        if os.path.exists(to_file):
            mode = "a"
        else:
            mode = "w+"

        with open(to_file, mode) as outfile:
            json.dump(app_dict, outfile)

    except IOError:
        print('An error occurred trying to read the file.')


_init()




