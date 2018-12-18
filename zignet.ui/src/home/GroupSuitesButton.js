import React, { Component } from 'react';

class GroupSuitesButton extends Component {

  render() {
    return (
      <a href="/?group=true" className="btn btn-primary" role="button" title="Click to group results by environment">Group</a>
    );
  }
}

export default GroupSuitesButton;
